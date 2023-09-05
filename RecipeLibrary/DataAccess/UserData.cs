using Dapper;
using RecipeLibrary.Models;
using System.Data;

namespace RecipeLibrary.DataAccess;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _sql;

    public UserData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    //POST
    public async Task<int> ShareRecipe(RecipeModel recipeModel)
    {
        var recipeIngredientsTable = new DataTable();
        recipeIngredientsTable.Columns.Add("IngredientName", typeof(string));
        recipeIngredientsTable.Columns.Add("Amount", typeof(decimal));
        recipeIngredientsTable.Columns.Add("Unit", typeof(string));

        foreach (var ingredient in recipeModel.RecipeIngredients)
        {
            recipeIngredientsTable.Rows.Add(ingredient.IngredientName, ingredient.Amount, ingredient.Unit);
        }

        var createdRecipeId = await _sql.LoadData<int, dynamic>(
            "spInsertRecipeWithIngredients",
            new
            {
                Name = recipeModel.Name,
                Description = recipeModel.Description,
                Instructions = recipeModel.Instructions,
                ImageUrl = recipeModel.ImageUrl,
                CreatedBy = recipeModel.CreatedBy,
                RecipeIngredients = recipeIngredientsTable.AsTableValuedParameter("RecipeIngredientType")
            },
            "Default");
        return createdRecipeId.FirstOrDefault();
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

    public async Task<List<int>> GetUserFavoritesIds(string userSub)
    {
        return await _sql.LoadData<int, dynamic>(
            "spGetUserFavoriteIds",
            new
            {
                UserSub = userSub
            },
            "Default");
    }

    public async Task<List<RecipeDto>> GetUserFavorites(string userSub)
    {
        var userFavoriteRecipes = await _sql.LoadData<RecipeDto, dynamic>(
            "spGetUserFavorites",
            new
            {
                UserSub = userSub
            },
            "Default");

        return userFavoriteRecipes;
    }
}
