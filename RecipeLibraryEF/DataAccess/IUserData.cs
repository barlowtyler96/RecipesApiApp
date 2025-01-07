using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;

namespace RecipeLibraryEF.DataAccess;

public interface IUserData
{
    Task AddNewUserAsync(User newUser);
    Task AddUserFavoriteAsync(UserFavorite userFavorite);
    Task DeleteUserFavoriteAsync(UserFavorite userFavorite);
    Task<PaginationResponse<List<RecipeDto>>> GetUserCreatedRecipesAsync(string sub, int page, int pageSize);
    Task<List<int>> GetUserFavoriteIdsAsync(string userSub);
    Task<PaginationResponse<List<RecipeDto>>> GetUserFavoriteRecipesAsync(string sub, int page, int pageSize);
}