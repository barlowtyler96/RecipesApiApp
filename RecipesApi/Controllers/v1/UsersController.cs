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
            Sub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!,
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
            _logger.LogError(ex, "The POST call to api/users/new failed. UserSub: {newUser.UserSub}", newUser.Sub);
            return BadRequest();
        }
    }

    // POST: api/users/favorite
    [HttpPost("favorite/{id}")]
    public async Task<ActionResult> PostUserFavorite(int id)
    {
        UserFavorite userFavorite = new()
        {
            RecipeId = id,
            Sub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!
        };

        try
        {
            _logger.LogInformation("POST: api/users/favorite");
            await _data.AddUserFavoriteAsync(userFavorite);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The POST call to api/users/favorite failed. UserId: {userFavoriteDto.UserId}." +
                "RecipeId: {userFavoriteDto.RecipeId}", userFavorite.Sub, userFavorite.RecipeId);
            return BadRequest();
        }
    }

    // DELETE: api/users/favorite
    [HttpDelete("favorite/{id}")]
    public async Task<ActionResult> DeleteUserFavorite(int id)
    {
        UserFavorite userFavorite = new()
        {
            RecipeId = id,
            Sub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value!
        };

        try
        {
            _logger.LogInformation("DELETE: api/users/favorite");
            await _data.DeleteUserFavoriteAsync(userFavorite);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The DELETE call to api/users/favorite failed. UserId: {userFavoriteDto.UserId}." +
                "RecipeId: {userFavoriteDto.RecipeId}", userFavorite.Sub, userFavorite.RecipeId);
            return BadRequest();
        }
    }

    // GET: api/v1/users/myrecipes
    [HttpGet("myrecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserSharedRecipes([FromQuery] int page, int pageSize)
    {
        _logger.LogInformation("GET: api/Users/myrecipes");
        var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        try
        {
            var output = await _data.GetUserCreatedRecipesAsync(userSub!, page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/myrecipes failed. UserSub: {userSub}", userSub);
            return BadRequest();
        }
    }

    // GET: api/v1/users/favorites?page=1&pageSize=8
    [HttpGet("favorites")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserFavoriteRecipes([FromQuery] int page, int pageSize)
    {
        _logger.LogInformation("GET: api/Users/favorites?page={}&pageSize={}", page, pageSize);
        var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        try
        {
            var output = await _data.GetUserFavoriteRecipesAsync(userSub!, page, pageSize);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/Users/favorites?page={currentPage}&pageSize={pageSize} failed. UserSub: {UserSub}", page, pageSize, userSub);
            return BadRequest();
        }
    }
}
