using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;

namespace RecipesApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeData _data;
    private readonly ILogger _logger;

    public RecipesController(IRecipeData data, ILogger<RecipesController> logger)
    {
        _data = data;
        _logger = logger;
    }

    // GET: api/v1/Recipes
    /// <summary>
    /// Gets a list of all RecipeDtos(paged)
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns>A list of paged recipe dtos</returns>
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> GetPagedRecipeDtos([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/Recipes?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {

            if (page > 0 && pageSize > 0)
            {
                var output = await _data.GetAllRecipeDtos(page, pageSize);
                return Ok(output);
            }
            else
            {
                return BadRequest(new
                {
                    message = $"The pageNumber or pageSize provided is not valid."
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes?page={page}&pageSize={pageSize}", page, pageSize);
            return BadRequest();
        }
    }

    // GET: api/v1/Recipes/full
    /// <summary>
    /// Gets a list of all RecipeModels(paged)
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns>A list of paged recipe models</returns>
    [HttpGet("full")]
    public async Task<ActionResult<PaginationResponse<List<RecipeModel>>>> GetPagedFullRecipes([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/v1/Recipes/full?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            if (page > 0 && pageSize > 0)
            {
                var output = await _data.GetAllRecipeModels(page, pageSize);
                return Ok(output);
            }
            else
            {
                return BadRequest(new
                {
                    message = $"The pageNumber or pageSize provided is not valid."
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/v1/Recipes/full?page={page}&pageSize={pageSize}", page, pageSize);
            return BadRequest();
        }
    }

    // GET: api/v1/Recipes/recent?page={page}&pageSize={pageSize}
    /// <summary>
    /// Gets a list of most recently added RecipeDtos(paged)
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns>A list of paged most recently added recipes</returns>
    [HttpGet("recent")]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> GetRecentRecipes([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/Recipes/recent?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            if (page > 0 && pageSize > 0)
            {
                var output = await _data.GetByDate(page, pageSize);
                return Ok(output);
            }
            else
            {
                return BadRequest(new
                {
                    message = $"The pageNumber or pageSize provided is not valid."
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes/recent?page={page}&pageSize={pageSize} failed.", page, pageSize);
            return BadRequest();
        }
    }

    // GET api/v1/Recipes/search?keyword={keyword}&page={page}&pageSize={pageSize}
    /// <summary>
    /// Gets a list of RecipeDtos based on a keyword(paged)
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns>A list of paged recipes based on a keyword</returns>
    [HttpGet("search")]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> GetByKeyword(
    [FromQuery] string keyword, [FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/Recipes/search?keyword={keyword}&page={page}&pageSize={pageSize}",
            keyword, page, pageSize);

        try
        {
            var output = await _data.GetByKeyword(keyword, page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "The GET call to api/Recipes/keyword={keyword}/page={page}/pageSize={pageSize} failed.",
                keyword, page, pageSize);
            return BadRequest();
        }
    }

    // GET api/v1/Recipes/5
    /// <summary>
    /// Gets a RecipeModel based on the id. Includes RecipeIngredients.
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns>A RecipeModel with the specified id. Includes RecipeIngredients</returns>
    [HttpGet("{recipeId}")]
    public async Task<ActionResult<RecipeModel>> GetById(int recipeId)
    {
        _logger.LogInformation("GET: api/Recipes/{RecipeId}", recipeId);

        try
        {
            var output = await _data.GetById(recipeId);
            if (output == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(output);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The GET call to api/Recipes/id/{RecipeId}", recipeId);
            return BadRequest();
        }
    }
}
