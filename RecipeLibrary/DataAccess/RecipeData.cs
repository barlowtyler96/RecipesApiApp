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
    public Task<List<RecipeDto>> GetAll()
    {
        return _sql.LoadData<RecipeDto, dynamic>(
            "dbo.spRecipes_GetAll",
            new { },
            "Default");
    }

    //GET
    public async Task<List<RecipeModel>> GetById(int id)
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
        var recipeModels = PopulateRecipeModels(recipesDto, recipeIngredients);

        return recipeModels;
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

    //PUT
    public Task UpdateAllColumns(int recipesId, RecipeDto recipeDto)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_UpdateAllColumns",
            new
            {
                RecipesId = recipesId,
                Name = recipeDto.Name,
                Description = recipeDto.Description,
                Instructions = recipeDto.Instructions,
                ImageUrl = recipeDto.ImageUrl
            },
            "Default");
    }

    //DELETE
    public Task Delete(int recipesId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_Delete",
            new
            { RecipesId = recipesId },
            "Default");
    }

    private List<RecipeModel> PopulateRecipeModels(List<RecipeDto> recipesDto, List<RecipeIngredient> recipeIngredients)
    {
        List<RecipeModel> recipeModels = recipesDto.Select(recipe => new RecipeModel
        {
            RecipeId = recipe.RecipeId,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            ImageUrl = recipe.ImageUrl,
            RecipeIngredients = recipeIngredients.Where(ri => ri.RecipeId == recipe.RecipeId).ToList()
        }).ToList();

        return recipeModels;
    }
}
