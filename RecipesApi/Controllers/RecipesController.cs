using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Constants;

namespace RecipesApi.Controllers;

[Authorize]
[RequiredScope(PolicyConstants.ReadScope)]
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
    public async Task<ActionResult<List<RecipeModel>>> Get()
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
    public async Task<ActionResult<List<RecipeModel>>> GetByDate()
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

    // GET api/Recipes/5
    [HttpGet("{recipeId}")]
    public async Task<ActionResult<RecipeModel>> Get(int recipeId)
    {
        _logger.LogInformation("GET: api/Recipes/{RecipeId}", recipeId);

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
