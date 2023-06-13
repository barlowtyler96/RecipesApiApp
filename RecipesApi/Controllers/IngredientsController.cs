using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.Models;
using RecipesApi.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IngredientsController : ControllerBase
{
    // GET: api/<IngredientController>
    [HttpGet]
    public ActionResult<IEnumerable<Ingredient>> Get()
    {
        throw new NotImplementedException();
    }

    // GET api/<IngredientController>/5
    [HttpGet("{id}")]
    public ActionResult<Ingredient> Get(int id)
    {
        throw new NotImplementedException();
    }

    // POST api/<IngredientController>
    [HttpPost]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public IActionResult Post([FromBody] string value)
    {
        throw new NotImplementedException();
    }

    // PUT api/<IngredientController>/5
    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public IActionResult Put(int id, [FromBody] string value)
    {
        throw new NotImplementedException();
    }

    // DELETE api/<IngredientController>/5
    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }
}
