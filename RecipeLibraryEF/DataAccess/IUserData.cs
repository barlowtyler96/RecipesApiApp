using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;

namespace RecipeLibraryEF.DataAccess;

public interface IUserData
{
    Task AddNewUserAsync(User newUser);
    Task AddUserFavoriteAsync(UserFavoriteDto userFavoriteDto);
    Task DeleteUserFavoriteAsync(UserFavoriteDto userFavoriteDto);
    Task<List<RecipeDto>> GetUserFavoriteRecipesAsync(string userSub);
    Task<List<int>> GetUserFavoriteIdsAsync(string userSub);
    Task<List<RecipeDto>> GetUserCreatedRecipesAsync(string userSub);
}