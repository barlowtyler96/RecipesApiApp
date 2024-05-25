using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;
using RecipeLibrary.Models;
using System.Reflection.PortableExecutable;

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

        var reader = await connection.QueryMultipleAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        List<T> response = reader.Read<T>().ToList();

        if (typeof(T) == typeof(RecipeModel))
        {
            var recipesIngredients = reader.Read<RecipeIngredient>().ToList();
            foreach (var recipe in response.Cast<RecipeModel>())
            {
                recipe.RecipeIngredients = recipesIngredients
                    .Where(ingredient => ingredient.RecipeId == recipe.RecipeId)
                    .ToList();
            }
        }
        return response;
    }

    public async Task<PaginationResponse<List<T>>> LoadPaginationData<T, U>(
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

        List<T> recipes = reader.Read<T>().ToList();

        if (typeof(T) == typeof(RecipeModel))
        {
            var recipesIngredients = reader.Read<RecipeIngredient>().ToList();
            foreach (var recipe in recipes.Cast<RecipeModel>())
            {
                recipe.RecipeIngredients = recipesIngredients
                    .Where(ingredient => ingredient.RecipeId == recipe.RecipeId)
                    .ToList();
            }
        }

        PaginationResponse<List<T>> response = new (totalCount, pageSize, currentPageNumber, recipes);

        return response;
    }

    public async Task<int> SaveData<T>(string storedProcedure,
        T parameters,
        string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        int rowsAffected = await connection.ExecuteAsync(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        return rowsAffected;
    }
}
