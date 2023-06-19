using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Constants;

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

    // GET api/Recipes
    [HttpGet("{id}")]
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

    // POST api/Recipes
    [HttpPost]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public async Task<ActionResult<RecipeModel>> Post([FromBody] RecipeModel recipeModel)
    {
        _logger.LogInformation("POST: api/Recipes");
        try
        {
            var output = await _data.Create(recipeModel.Name, recipeModel.Description,
                                            recipeModel.Ingredients, recipeModel.Instructions);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The POST call to api/Recipes failed. Recipe model was " +
                "Name: {Name} Description: {Description} Ingredients: {Ingredients} Instructions: {Instructions}",
                recipeModel.Name, recipeModel.Description, recipeModel.Ingredients, recipeModel.Instructions);
            return BadRequest();
        }
    }

    // PUT api/Recipes/5
    [HttpPut("{recipeId}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public async Task<ActionResult> Put(int recipeId, [FromBody] RecipeModel recipeModel)
    {
        _logger.LogInformation("PUT: api/Recipes/{RecipeId}", recipeId);

        try
        {
            await _data.UpdateAllColumns(recipeId, recipeModel.Name, recipeModel.Description,
                                                recipeModel.Ingredients, recipeModel.Instructions);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The PUT call to api/Recipes/ failed. Recipe model was " +
                "Name: {Name} Description: {Description} Ingredients: {Ingredients} Instructions: {Instructions}",
                recipeModel.Name, recipeModel.Description, recipeModel.Ingredients, recipeModel.Instructions);
            return BadRequest();
        }
    }

    // DELETE api/Recipes/5
    [HttpDelete("{recipeId}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public async Task<IActionResult> Delete(int recipeId)
    {
        _logger.LogInformation("DELETE: api/Recipes/{RecipeId}", recipeId);

        try
        {
            await _data.Delete(recipeId);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("The DELETE call to api/Recipes/{RecipeId} failed", recipeId);
            return BadRequest();
        }
    }
}
