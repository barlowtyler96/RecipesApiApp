﻿using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess;

public class RecipeData : IRecipeData
{
    private readonly ISqlDataAccess _sql;

    public RecipeData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetAllRecipeDtos(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        return await _sql.LoadPaginationData<RecipeDto, dynamic>(
            "dbo.spGetAllRecipeDtos",
            new { Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
    }

    public async Task<PaginationResponse<List<RecipeModel>>> GetAllRecipeModels(int currentPageNumber, int pageSize) 
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        return await _sql.LoadPaginationData<RecipeModel, dynamic>(
            "dbo.spGetAllRecipes",
            new { Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
    }

    //GET
    public async Task<RecipeModel> GetById(int id)
    {
        var response = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spGetRecipeById",
            new { RecipeId = id },
            "Default"); 

        return response.FirstOrDefault()!;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetByDate(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        var results =  await _sql.LoadPaginationData<RecipeDto, dynamic>(
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

        var results = await _sql.LoadPaginationData<RecipeDto, dynamic>(
            "dbo.spGetRecipesByKeyword",
            new { Keyword = keyword, Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);

        return results;
    }
}
