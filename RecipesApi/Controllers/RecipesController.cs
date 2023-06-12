using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.Models;
using RecipesApi.Constants;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    // GET: api/Recipes
    [HttpGet]
    public ActionResult<IEnumerable<RecipeModel>> Get()
    {
        throw new NotImplementedException();
    }

    // GET api/Recipes
    [HttpGet("{id}")]
    public ActionResult<RecipeModel> Get(int id)
    {
        throw new NotImplementedException();
    }

    // POST api/Recipes
    [HttpPost]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public IActionResult Post([FromBody] string value)
    {
        throw new NotImplementedException();
    }

    // PUT api/Recipes/5
    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
    public IActionResult Put(int id, [FromBody] string value)
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
