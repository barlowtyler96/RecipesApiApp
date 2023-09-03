using RecipeLibrary.Models;
namespace RecipeLibrary.DataAccess;

public interface IUserData
{
    Task AddUserFavorite(UserFavorite userFavorite);
    Task DeleteUserFavorite(UserFavorite userFavorite);
    Task<List<RecipeDto>> GetUserFavorites(string userSub);
    Task<List<int>> GetUserFavoritesIds(string userSub);
}