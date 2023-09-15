using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;

namespace RecipesApi.Controllers;

[Route("api/[controller]")]
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

    // GET: api/Recipes
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> Get([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/Recipes?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            var output = await _data.GetAll(page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes?page={page}&pageSize={pageSize}", page, pageSize);
            return BadRequest();
        }
    }

    // GET: api/Recipes/recent?page={page}&pageSize={pageSize}
    [HttpGet("recent")]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> GetRecentRecipes([FromQuery] int page, [FromQuery] int pageSize)
    {
        _logger.LogInformation("GET: api/Recipes/recent?page={page}&pageSize={pageSize}", page, pageSize);

        try
        {
            var output = await _data.GetByDate(page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes/recent?page={page}&pageSize={pageSize} failed.", page, pageSize);
            return BadRequest();
        }
    }

    // GET api/Recipes/search?keyword={keyword}&page={page}&pageSize={pageSize}
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

    // GET api/Recipes/id/5
    [HttpGet("id/{recipeId}")]
    public async Task<ActionResult<RecipeModel>> Get(int recipeId)
    {
        _logger.LogInformation("GET: api/Recipes/id/{RecipeId}", recipeId);

        try
        {
            var output = await _data.GetById(recipeId);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The GET call to api/Recipes/id/{RecipeId}", recipeId);
            return NotFound(new
            {
                message = $"The recipe with ID {recipeId} was not found."
            });
        }
    }
}
