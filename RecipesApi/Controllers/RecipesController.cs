using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<List<RecipeDto>>> Get()
    {
        _logger.LogInformation("GET: api/Recipes");

        try
        {
            var output = await _data.GetAll();
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes failed.");
            return BadRequest();
        }
    }

    // GET: api/Recipes/recent
    [HttpGet("recent")]
    public async Task<ActionResult<List<RecipeDto>>> GetRecentRecipes()
    {
        _logger.LogInformation("GET: api/Recipes/recent");

        try
        {
            var output = await _data.GetByDate();
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Recipes/recent failed.");
            return BadRequest();
        }
    }

    // GET: api/Recipes/keyword={keyword}/page={currentPageNumber}/pageSize={pageSize}
    [HttpGet("keyword={keyword}/page={currentPageNumber}/pageSize={pageSize}")]
    public async Task<ActionResult<PaginationResponse<List<RecipeDto>>>> GetByKeyword(
        string keyword, int currentPageNumber, int pageSize)
    {
        _logger.LogInformation("GET: api/Recipes/keyword={keyword}/page={currentPageNumber}/pageSize={pageSize}", 
            keyword, currentPageNumber, pageSize);

        try
        {
            var output = await _data.GetByKeyword(keyword, currentPageNumber, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "The GET call to api/Recipes/keyword={keyword}/page={currentPageNumber}/pageSize={pageSize} failed.", 
                keyword, currentPageNumber, pageSize);
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
                "The GET call to {ApiPath} failed. The Id was {RecipeId}", $"api/Recipes/Id", recipeId);
            return BadRequest();
        }
    }
}
