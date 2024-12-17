using Microsoft.AspNetCore.Mvc;
using RecipeLibraryEF.DataAccess;
using RecipeLibraryEF.Models.Dtos;
using RecipesApi.Services;

namespace RecipesApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeData _data;
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBlobService _blobService;

    public RecipesController(IRecipeData data, ILogger<RecipesController> logger, IHttpContextAccessor httpContextAccessor, IBlobService blobService)
    {
        _data = data;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _blobService = blobService;
    }

    // GET: api/recipes?page={page}&pageSize={pagesize}
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> Get([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/recipes/?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            var output = await _data.GetAllRecipesAsync(page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/recipes?page={page}&pageSize={pageSize} failed", page, pageSize);
            return BadRequest();
        }
    }

    // GET api/recipes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeDto>> GetById(int id)
    {
        _logger.LogInformation("GET: api/v1/recipes/{id}", id);

        try
        {
            var output = await _data.GetByIdAsync(id);
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

    //GET api/recipes/search?keyword={keyword}&page={page}&pageSize={pageSize}
    [HttpGet("search")]
    public async Task<ActionResult<RecipeDto>> GetByKeyword([FromQuery] string keyword, [FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/recipes/keyword?keyword={keyword}&page={page}&pageSize={pageSize}", keyword, page, pageSize);

        try
        {
            var output = await _data.GetByKeywordAsync(keyword, page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/recipes/search?keyword={keyword}&page={page}&pageSize={pageSize} failed", keyword, page, pageSize);
            return BadRequest();
        }
    }

    // POST api/recipes/share
    [HttpPost("share")]
    public async Task<ActionResult<RecipeDto>> Post([FromBody] RecipeDto newRecipeDto)
    {
        _logger.LogInformation("POST: api/v1/recipes/share");
        newRecipeDto.CreatedBySub = "47678e37-977b-4665-9451-88c53d5c65d0";
        newRecipeDto.CreatedOn = DateTime.UtcNow;
        //newRecipeDto.CreatedBy = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!;

        try
        {
            var createdRecipe = await _data.AddRecipeAsync(newRecipeDto);
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

    // DELETE api/Recipes
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _data.DeleteRecipeAsync(id);
    }
}
