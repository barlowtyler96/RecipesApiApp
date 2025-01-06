using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipeLibraryEF.DataAccess;
using RecipeLibraryEF.Models.Dtos;
using RecipesApi.Constants;
using RecipesApi.Services;

namespace RecipesApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeData _recipeData;
    private readonly IUserData _userData;
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBlobService _blobService;

    public RecipesController(IRecipeData recipeData, IUserData userData, ILogger<RecipesController> logger, IHttpContextAccessor httpContextAccessor, IBlobService blobService)
    {
        _recipeData = recipeData;
        _userData = userData;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _blobService = blobService;
    }

    // GET: api/v1/recipes?page={page}&pageSize={pagesize}
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> Get([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/recipes/?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!;
            var recipes = await _recipeData.GetRecipesAsync(page, pageSize, userSub);
            
            return Ok(recipes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/recipes?page={page}&pageSize={pageSize} failed", page, pageSize);
            return BadRequest();
        }
    }

    // GET: api/v1/recipes/recent?page={page}&pageSize={pagesize}
    [HttpGet("recent")]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> GetRecents([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/recipes/?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!;
            var recipes = await _recipeData.GetRecipesRecentAsync(page, pageSize, userSub);

            return Ok(recipes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/recipes?page={page}&pageSize={pageSize} failed", page, pageSize);
            return BadRequest();
        }
    }

    // GET api/v1/recipes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetById(int id)
    {
        _logger.LogInformation("GET: api/v1/recipes/{id}", id);

        try
        {
            var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!;
            var output = await _recipeData.GetByIdAsync(id, userSub);

            if (output == null)
            {
                return NotFound(new { Message = $"Recipe with the id: {id} not found" });
            }
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/recipes/{}id failed", id);
            return BadRequest();
        }
    }

    //GET api/v1/recipes/search?keyword={keyword}&page={page}&pageSize={pageSize}
    [HttpGet("search")]
    public async Task<ActionResult<RecipeDto>> GetByKeyword([FromQuery] string keyword, [FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/recipes/keyword?keyword={keyword}&page={page}&pageSize={pageSize}", keyword, page, pageSize);

        try
        {
            var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!;
            var output = await _recipeData.GetByKeywordAsync(keyword, page, pageSize, userSub);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/recipes/search?keyword={keyword}&page={page}&pageSize={pageSize} failed", keyword, page, pageSize);
            return BadRequest();
        }
    }

    // POST api/v1/recipes/share
    [Authorize]
    [RequiredScope(ScopeConstants.UserReadWrite)]
    [HttpPost("share")]
    public async Task<ActionResult<RecipeDto>> Post([FromForm] RecipeDto newRecipeDto)
    {
        _logger.LogInformation("POST: api/v1/recipes/share");
        newRecipeDto.CreatedBy.Sub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!;

        try
        {
            var createdRecipe = await _recipeData.AddRecipeAsync(newRecipeDto);
            var uri = "api/Recipes/" + createdRecipe.Id;
            return Created(uri, createdRecipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST call to api/recipes/share failed");
            return BadRequest(ModelState);
        }
    }

    // POST: api/v1/recipes/upload
    [Authorize]
    [RequiredScope(ScopeConstants.UserReadWrite)]
    [HttpPost("upload"), DisableRequestSizeLimit()]
    public async Task<ActionResult> PostRecipeImage()
    {
        IFormFile file = Request.Form.Files[0];
        if (file == null)
        {
            return BadRequest();
        }

        var result = await _blobService.UploadFileBlobAsync(
                "recipevaultimages",
                file.OpenReadStream(),
                file.ContentType,
                file.Name);

        var toReturn = result.AbsoluteUri;

        return Ok(new { path = toReturn });
    }
}
