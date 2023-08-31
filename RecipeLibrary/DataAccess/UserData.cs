using RecipeLibrary.Models;
namespace RecipeLibrary.DataAccess;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _sql;

    public UserData(ISqlDataAccess sql)
    {
        _sql = sql;
    }


    public async Task AddUserFavorite(UserFavorite userFavorite)
    {
        await _sql.SaveData<dynamic>(
            "spAddUserFavorite",
            new
            {
                UserSub = userFavorite.UserSub,
                RecipeId = userFavorite.RecipeId
            },
            "Default");
    }

    public async Task DeleteUserFavorite(UserFavorite userFavorite) 
    {
        await _sql.SaveData<dynamic>(
            "spDeleteUserFavorite",
            new
            {
                UserSub = userFavorite.UserSub,
                RecipeId = userFavorite.RecipeId
            },
            "Default");
    }

    public async Task<List<UserFavorite>> GetUserFavoritesIds(string userSub)
    {
        return await _sql.LoadData<UserFavorite, dynamic>(
            "spGetUserFavoriteIds",
            new
            {
                UserSub = userSub
            },
            "Default");
    }
}
