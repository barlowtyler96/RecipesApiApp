using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipeLibraryEF.DataAccess;
using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;
using RecipesApi.Constants;

namespace RecipesApi.Controllers.v1;

[Authorize]
[RequiredScope(ScopeConstants.UserReadWrite)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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

    // POST api/v1/Users/new
    [HttpPost("new")]
    public async Task<ActionResult> PostNewUser()
    {
        _logger.LogInformation("POST: api/users/new");

        var newUser = new User()
        {
            UserSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!,
            FirstName = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "extension_FirstName")?.Value!,
            LastName = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "extension_LastName")?.Value!
        };

        try
        {
            await _data.AddNewUserAsync(newUser);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST call to api/users/new failed. UserSub: {newUser.UserSub}", newUser.UserSub);
            return BadRequest();
        }
    }

    // POST: api/users/favorite
    [HttpPost("favorite/{id}")]
    public async Task<ActionResult> PostUserFavorite(int recipeId)
    {
        UserFavoriteDto userFavoriteDto = new()
        {
            RecipeId = recipeId,
            UserSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!
        };

        try
        {
            _logger.LogInformation("POST: api/users/favorite");
            await _data.AddUserFavoriteAsync(userFavoriteDto);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST call to api/users/favorite failed. UserId: {userFavoriteDto.UserId}." +
                "RecipeId: {userFavoriteDto.RecipeId}", userFavoriteDto.UserSub, userFavoriteDto.RecipeId);
            return BadRequest();
        }
    }

    // DELETE: api/users/favorite
    [HttpDelete("favorite/{id}")]
    public async Task<ActionResult> DeleteUserFavorite(int recipeId)
    {
        UserFavoriteDto userFavoriteDto = new()
        {
            RecipeId = recipeId,
            UserSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!
        };

        try
        {
            _logger.LogInformation("DELETE: api/users/favorite");
            await _data.DeleteUserFavoriteAsync(userFavoriteDto);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The DELETE call to api/users/favorite failed. UserId: {userFavoriteDto.UserId}." +
                "RecipeId: {userFavoriteDto.RecipeId}", userFavoriteDto.UserSub, userFavoriteDto.RecipeId);
            return BadRequest();
        }
    }

    // GET: api/v1/users/myrecipes
    [HttpGet("myrecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserSharedRecipes()
    {
        _logger.LogInformation("GET: api/Users/myrecipes");
        var userSub = "47678e37-977b-4665-9451-88c53d5c65d0";
        //var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        try
        {
            var output = await _data.GetUserCreatedRecipesAsync(userSub!);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/myrecipes failed. UserSub: {userSub}", userSub);
            return BadRequest();
        }
    }

    // GET: api/v1/users/favorites
    [HttpGet("favorites")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserFavoriteRecipes()
    {
        _logger.LogInformation("GET: api/Users/favorites");
        var userSub = "47678e37-977b-4665-9451-88c53d5c65d0";
        //var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        try
        {
            var output = await _data.GetUserFavoriteRecipesAsync(userSub!);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/favorites failed. UserSub: {UserSub}", userSub);
            return BadRequest();
        }
    }
}
