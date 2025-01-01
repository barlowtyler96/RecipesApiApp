using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;

namespace RecipeLibraryEF.DataAccess;

public interface IUserData
{
    Task AddNewUserAsync(User newUser);
    Task AddUserFavoriteAsync(UserFavorite userFavorite);
    Task DeleteUserFavoriteAsync(UserFavorite userFavorite);
    Task<List<RecipeDto>> GetUserFavoriteRecipesAsync(string userSub);
    Task<List<int>> GetUserFavoriteIdsAsync(string userSub);
    Task<List<RecipeDto>> GetUserCreatedRecipesAsync(string userSub);
}