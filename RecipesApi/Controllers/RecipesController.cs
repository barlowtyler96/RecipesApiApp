using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    // GET: api/Recipes
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/Recipes
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/Recipes
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/Recipes/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/Recipes/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
