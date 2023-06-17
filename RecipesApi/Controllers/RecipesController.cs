using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Constants;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
    [HttpGet("{name}")]
    public async Task<ActionResult<RecipeModel>> Get(string name)
    {
        var output = await _data.GetByName(name);
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
    public IActionResult Put([FromBody] RecipeModel recipeModel)
    {
        throw new NotImplementedException();
    }

    // DELETE api/Recipes/5
    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}
