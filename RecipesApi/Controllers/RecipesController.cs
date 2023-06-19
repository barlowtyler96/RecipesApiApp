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

    public RecipesController(IRecipeData data)
    {
        _data = data;
    }

    // GET: api/Recipes
    [HttpGet]
    public async Task<ActionResult<List<RecipeModel>>> Get()
    {
        var output = await _data.GetAll();
        return Ok(output);
    }

    // GET api/Recipes
    [HttpGet("{id}")]
    public async Task<ActionResult<RecipeModel>> Get(int id)
    {
        var output = await _data.GetById(id);
        return Ok(output);
    }

    // POST api/Recipes
    [HttpPost]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public async Task<ActionResult<RecipeModel>> Post([FromBody] RecipeModel recipeModel)
    {
        var output = await _data.Create(recipeModel.Name, recipeModel.Description, 
                                        recipeModel.Ingredients, recipeModel.Instructions);
        return Ok(output);
    }

    // PUT api/Recipes/5
    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public async Task<ActionResult> Put(int id, [FromBody] RecipeModel recipeModel)
    {
        await _data.UpdateAllColumns(id, recipeModel.Name, recipeModel.Description,
                                            recipeModel.Ingredients, recipeModel.Instructions);
        return Ok();
    }

    // DELETE api/Recipes/5
    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public async Task<IActionResult> Delete(int id)
    {
        await _data.Delete(id);

        return Ok();
    }
}
