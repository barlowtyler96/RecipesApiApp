using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;

namespace RecipesApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
	[ApiController]
    public class AdministratorController : ControllerBase
	{
		private readonly IRecipeData _data;
		private readonly ILogger _logger;
		public AdministratorController(IRecipeData data, ILogger<AdministratorController> logger)
		{
			_data = data;
			_logger = logger;
		}

		// POST api/Administrator
		[HttpPost]
		public async Task<ActionResult<int>> Post([FromBody] RecipeModel recipeDto)
		{
			_logger.LogInformation("POST: api/Administrator");
			try
			{
				var output = await _data.Create(recipeDto);
				return Ok(output);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"The POST call to api/Administrator failed. Recipe model was " +
					"Name: {Name} Description: {Description} Instructions: {Instructions}",
					recipeDto.Name, recipeDto.Description, recipeDto.Instructions);
				return BadRequest();
			}
		}

		// PUT api/Administrator/5
		[HttpPut("{recipeId}")]
		public async Task<ActionResult> Put(int recipeId, [FromBody] RecipeDto recipeModel)
		{
			_logger.LogInformation("PUT: api/Administrator/{RecipeId}", recipeId);

			try
			{
				await _data.UpdateAllColumns(recipeId, recipeModel);
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"The PUT call to api/Administrator/{RecipeId} failed. Recipe model was " +
					"Name: {Name} Description: {Description} Instructions: {Instructions}",
					recipeId, recipeModel.Name, recipeModel.Description, recipeModel.Instructions);
				return BadRequest();
			}
		}

		// DELETE api/Administrator/5
		[HttpDelete("{recipeId}")]
		public async Task<IActionResult> Delete(int recipeId)
		{
			_logger.LogInformation("DELETE: api/Administrator/{RecipeId}", recipeId);

			try
			{
				await _data.Delete(recipeId);

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "The DELETE call to api/Administrator/{RecipeId} failed", recipeId);
				return BadRequest();
			}
		}
	}
}
