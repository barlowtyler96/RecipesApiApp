using RecipeLibrary.Models;
using System.Data;

namespace RecipeLibrary.DataAccess;

public class RecipeData : IRecipeData
{
    private readonly ISqlDataAccess _sql;

    public RecipeData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    //GET
    public Task<PaginationResponse<List<RecipeDto>>> GetAll(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        return _sql.LoadMultiData<RecipeDto, dynamic>(
            "dbo.spGetRecipes_All",
            new { Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
    }

    //GET
    public async Task<RecipeModel> GetById(int id)
    {
        var recipesDto = await _sql.LoadData<RecipeDto, dynamic>(
            "dbo.spGetRecipeById",
            new 
            { RecipeId = id},
            "Default");

        var recipeId = recipesDto[0].RecipeId;
        var recipeIdsAsString = string.Join(",", recipeId);
        var recipeIngredients = await _sql.LoadData<RecipeIngredient, dynamic>(
            "dbo.spGetRecipeIngredientsById",
            new { RecipeIdsAsString = recipeIdsAsString },
            "Default");

        var ingredientIdsAsString = string.Join(",", recipeIngredients.Select(i => i.IngredientId));
        var ingredients = await _sql.LoadData<IngredientModel, dynamic>(
            "dbo.spGetIngredientsById",
            new { IngredientIdsAsString = ingredientIdsAsString },
            "Default");

        for (int i = 0; i < ingredients.Count && i < recipeIngredients.Count; i++)
        {
            recipeIngredients[i].IngredientName = ingredients[i].Name;
        }
        var recipeModel = new RecipeModel(recipesDto.FirstOrDefault()!, recipeIngredients);

        return recipeModel;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetByDate(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        var results =  await _sql.LoadMultiData<PaginationResponse<RecipeDto>, dynamic>(
            "dbo.spGetRecipesByDate",
            new { Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
    
        return results;
    }

    public async Task<PaginationResponse<List<RecipeDto>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        var results = await _sql.LoadMultiData<PaginationResponse<RecipeDto>, dynamic>(
            "dbo.spGetRecipesByKeyword",
            new { Keyword = keyword, Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);

        return results;
    }
}
