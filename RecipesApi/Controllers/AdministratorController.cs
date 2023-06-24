using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipesApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdministratorController : ControllerBase
	{
		private readonly IRecipeData _data;
		private readonly ILogger _logger;
		public AdministratorController(IRecipeData data, ILogger<RecipesController> logger)
		{
			_data = data;
			_logger = logger;
		}

		// POST api/Administrator
		[HttpPost]
		[Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
		public async Task<ActionResult<RecipeModel>> Post([FromBody] RecipeModel recipeModel)
		{
			_logger.LogInformation("POST: api/Administrator");
			try
			{
				var output = await _data.Create(recipeModel);
				return Ok(output);
			}
			catch (Exception ex)
			{
				_logger.LogError(
					ex,
					"The POST call to api/Administrator failed. Recipe model was " +
					"Name: {Name} Description: {Description} Ingredients: {Ingredients} Instructions: {Instructions}",
					recipeModel.Name, recipeModel.Description, recipeModel.Ingredients, recipeModel.Instructions);
				return BadRequest();
			}
		}

		// PUT api/Administrator/5
		[HttpPut("{recipeId}")]
		[Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
		public async Task<ActionResult> Put(int recipeId, [FromBody] RecipeModel recipeModel)
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
					"Name: {Name} Description: {Description} Ingredients: {Ingredients} Instructions: {Instructions}",
					recipeId, recipeModel.Name, recipeModel.Description, recipeModel.Ingredients, recipeModel.Instructions);
				return BadRequest();
			}
		}

		// DELETE api/Administrator/5
		[HttpDelete("{recipeId}")]
		[Authorize(Policy = PolicyConstants.MustBeAnAdmin)]
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
