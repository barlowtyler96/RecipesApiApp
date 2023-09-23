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
    public Task<PaginationResponse<List<RecipeDto>>> GetAllRecipeDtos(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        return _sql.LoadPaginationData<RecipeDto, dynamic>(
            "dbo.spGetAllRecipeDtos",
            new { Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
    }

    public Task<PaginationResponse<List<RecipeModel>>> GetAllRecipeModels(int currentPageNumber, int pageSize) 
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        return _sql.LoadRecipeModelData<RecipeModel, dynamic>(
            "dbo.spGetAllRecipes",
            new { Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
    }

    //GET
    public async Task<RecipeModel> GetById(int id)
    {
        var recipeModel = await _sql.LoadRecipeModelData<RecipeModel, dynamic>(
            "dbo.spGetRecipeById",
            new { RecipeId = id },
            "Default"); 

        return recipeModel;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetByDate(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        var results =  await _sql.LoadPaginationData<PaginationResponse<RecipeDto>, dynamic>(
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

        var results = await _sql.LoadPaginationData<PaginationResponse<RecipeDto>, dynamic>(
            "dbo.spGetRecipesByKeyword",
            new { Keyword = keyword, Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);

        return results;
    }
}
