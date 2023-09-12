using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
namespace RecipesApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserData _data;
    private readonly ILogger<UsersController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsersController(IUserData data, ILogger<UsersController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _data = data;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    // POST api/Users/new
    [HttpPost("new")]
    public async Task<ActionResult> PostNewUser()
    {
        var newUser = new UserModel()
        {
            UserSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
            FirstName = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "extension_FirstName")?.Value,
            LastName = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "extension_LastName")?.Value
        };

        _logger.LogInformation("POST: api/Users/new");
        try
        {
            await _data.AddNewUser(newUser);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The POST call to api/Users/new failed");
            return BadRequest();
        }
    }

    // POST api/Users/share
    [HttpPost("share")]
    public async Task<ActionResult<int>> PostSharedRecipe([FromBody] RecipeModel recipeModel)
    {
        recipeModel.CreatedBy = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        _logger.LogInformation("POST: api/Users/share");
        try
        {
            var output = await _data.ShareRecipe(recipeModel);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The POST call to api/Users/share failed. Recipe model was " +
                "Name: {Name} Description: {Description} Instructions: {Instructions}",
                recipeModel.Name, recipeModel.Description, recipeModel.Instructions);
            return BadRequest();
        }
    }

    // POST: api/Users/favorite
    [HttpPost("favorite")]
    public async Task<ActionResult> PostUserFavorite([FromBody] int recipeId)
    {
        var userFavorite = new UserFavorite()
        {
            UserSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
            RecipeId = recipeId
        };

        _logger.LogInformation("POST: api/Users/favorite");
        try
        {
            await _data.AddUserFavorite(userFavorite);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "The POST call to api/User/Favorite failed. UserFavorite model was " +
                "UserSub: {UserSub} RecipeId: {RecipeId}",
                userFavorite.UserSub, userFavorite.RecipeId);
            return BadRequest();
        }
    }

    // DELETE: api/Users/favorite/recipeId
    [HttpDelete("favorite/{recipeId}")]
    public async Task<ActionResult> DeleteUserFavorite(int recipeId)
    {
        var userFavorite = new UserFavorite()
        {
            UserSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value,
            RecipeId = recipeId
        };

        _logger.LogInformation("POST: api/Users/favorite/{recipeId}", recipeId);
        try
        {
            await _data.DeleteUserFavorite(userFavorite);
            return Ok();
        }
        catch (Exception ex)   
        {
            _logger.LogError(
                ex,
                "The DELETE call to api/Users/Favorite/recipeId failed. UserFavorite model was " +
                "UserSub: {UserSub} RecipeId: {RecipeId}",
                userFavorite.UserSub, userFavorite.RecipeId);
            return BadRequest();
        }
    }

    // GET: api/Users/myrecipes
    [HttpGet("myrecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserCreatedRecipes()
    {
        var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        _logger.LogInformation("GET: api/Users/myrecipes");

        try
        {
            var output = await _data.GetUserCreatedRecipes(userSub!);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/myrecipes failed.");
            return BadRequest();
        }
    }

    // GET: api/Users/favorites
    [HttpGet("favorites")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserFavorites()
    {
        var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        _logger.LogInformation("GET: api/Users/favorites");

        try
        {
            var output = await _data.GetUserFavorites(userSub!);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/favorites failed.");
            return BadRequest();
        }
    }

    // GET: api/Users/favoritesIds
    [HttpGet("favoritesIds")]
    public async Task<ActionResult<List<int>>> GetUserFavoritesIds()
    {
        var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        _logger.LogInformation("GET: api/Users/favoritesIds");

        try
        {
            var output = await _data.GetUserFavoritesIds(userSub!);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/favoritesIds failed.");
            return BadRequest();
        }
    }
}
