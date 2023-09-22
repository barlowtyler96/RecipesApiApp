﻿using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> LoadData<T, U>(
        string storedProcedure,
        U parameters,
        string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure); 

        return rows.ToList();

    }

    public async Task<PaginationResponse<List<RecipeDto>>> LoadPaginationData<T, U>(
        string storedProcedure,
        U parameters,
        string connectionStringName,
        int currentPageNumber, int pageSize)
    {
        string connectionString = _config.GetConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        var reader = await connection.QueryMultipleAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        int totalCount = reader.Read<int>().FirstOrDefault();
        
        List<RecipeDto> recipes = reader.Read<RecipeDto>().ToList();

        var response = new PaginationResponse<List<RecipeDto>>(totalCount, recipes, currentPageNumber, pageSize);

        return response;
    }

    public async Task<RecipeModel> LoadRecipeModelData<T, U>(
    string storedProcedure,
    U parameters,
    string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        var reader = await connection.QueryMultipleAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        RecipeDto recipeDto = reader.Read<RecipeDto>().FirstOrDefault()!;
        List<RecipeIngredient> recipeIngredients = reader.Read<RecipeIngredient>().ToList();

        RecipeModel recipeModel = new RecipeModel(recipeDto, recipeIngredients);

        return recipeModel;
    }

    public async Task SaveData<T>(string storedProcedure,
        T parameters,
        string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}
