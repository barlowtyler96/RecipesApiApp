using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdministratorController(IRecipeData data, ILogger<AdministratorController> logger, 
			IHttpContextAccessor httpContextAccessor)
		{
			_data = data;
			_logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

		// POST api/Administrator
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] RecipeModel recipeModel)
		{
			recipeModel.CreatedBy = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

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
					"Name: {Name} Description: {Description} Instructions: {Instructions}",
					recipeModel.Name, recipeModel.Description, recipeModel.Instructions);
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
