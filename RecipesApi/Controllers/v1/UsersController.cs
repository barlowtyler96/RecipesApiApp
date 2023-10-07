using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using RecipeLibrary.DataAccess;
using RecipeLibrary.Models;
using RecipesApi.Constants;
using RecipesApi.Services;

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
    private IBlobService _blobService;

    public UsersController(IUserData data, ILogger<UsersController> logger, IHttpContextAccessor httpContextAccessor, IBlobService blobService)
    {
        _data = data;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _blobService = blobService;
    }

    // POST api/v1/Users/new
    /// <summary>
    /// Posts a new users info to SQL database.(requires auth)
    /// </summary>
    /// <returns></returns>
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

    // POST api/v1/Users/share
    /// <summary>
    /// Posts a user's shared recipe to SQL database.(requires auth)
    /// </summary>
    /// <param name="recipeModel"></param>
    /// <returns>The new id of user's posted recipe</returns>
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

    // POST: api/v1/Users/favorite
    /// <summary>
    /// Posts a user's favorited recipe Id to SQL database.(requires auth)
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
    [HttpPost("favorite/{recipeId}")]
    public async Task<ActionResult> PostUserFavorite(int recipeId)
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

    // DELETE: api/v1/Users/favorite/recipeId
    /// <summary>
    /// Deletes a user's favorited recipe Id from SQL database.(requires auth)
    /// </summary>
    /// <param name="recipeId"></param>
    /// <returns></returns>
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

    // GET: api/v1/Users/myrecipes
    /// <summary>
    /// Gets a list of RecipeDtos a user has shared.(requires auth)
    /// </summary>
    /// <returns>A list of RecipeDtos a user has shared</returns>
    [HttpGet("myrecipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserSharedRecipes()
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

    // GET: api/v1/Users/favorites
    /// <summary>
    /// Gets a list of user's favorited RecipeDtos.(requires auth)
    /// </summary>
    /// <returns>A list of user's favorited RecipeDtos</returns>
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

    // GET: api/v1/Users/favoritesIds
    /// <summary>
    /// Gets a list of user's favorite recipe Ids.(requires auth)
    /// </summary>
    /// <returns>A list of recipe Ids</returns>
    [HttpGet("favoritesIds")]
    public async Task<ActionResult<List<int>>> GetUserFavoritesIds()
    {
        var userSub = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        _logger.LogInformation("GET: api/v1/Users/favoritesIds");

        try
        {
            var output = await _data.GetUserFavoritesIds(userSub!);
            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The GET call to api/v1/Users/favoritesIds failed.");
            return BadRequest();
        }
    }

    [HttpPost("upload"), DisableRequestSizeLimit()]
    public async Task<ActionResult> PostRecipeImage()   
    {
        IFormFile file = Request.Form.Files[0];
        if (file == null)
        {
            return BadRequest();
        }

        var result = await _blobService.UploadFileBlobAsync(
                "recipevaultimages",
                file.OpenReadStream(),
                file.ContentType,
                file.Name);

        var toReturn = result.AbsoluteUri;

        return Ok(new { path = toReturn });
    }

}
