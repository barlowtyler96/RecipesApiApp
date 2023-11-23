using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using RecipeLibrary.Models;
using static Dapper.SqlMapper;

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

        int totalCount = ReadTotalCount(reader);

        List<RecipeDto> recipes = ReadRecipeDtos(reader);

        PaginationResponse<List<RecipeDto>> response = new (totalCount, recipes, currentPageNumber, pageSize);

        return response;
    }

    public async Task<PaginationResponse<List<RecipeModel>>> LoadPaginationRecipeModelData<T, U>(   
        string storedProcedure,
        U parameters,
        string connectionStringName,
        int currentPageNumber, int pageSize)
    {
        string connectionString = _config.GetConnectionString(connectionStringName)!;

        using IDbConnection connection = new SqlConnection(connectionString);

        var reader = await connection.QueryMultipleAsync(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        int totalCount = ReadTotalCount(reader);
        List<RecipeDto> recipeDtos = ReadRecipeDtos(reader);
        List<RecipeIngredient> recipeIngredients = ReadRecipeIngredients(reader);

        List<RecipeModel> recipeModels = new ();

        foreach(var recipe in recipeDtos)
        {
            RecipeModel recipeModel = new (recipe, recipeIngredients);
            recipeModels.Add(recipeModel);
        }
        
        PaginationResponse<List<RecipeModel>> response = new (totalCount, recipeModels, currentPageNumber, pageSize);

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

        RecipeDto recipeDto = ReadRecipeDtos(reader).FirstOrDefault()!;
        List<RecipeIngredient> recipeIngredients = ReadRecipeIngredients(reader);

        RecipeModel recipeModel = new (recipeDto, recipeIngredients);

        return recipeModel;
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

    public static int ReadTotalCount(GridReader reader)
    {
        var totalCount = reader.Read<int>();
        return totalCount.FirstOrDefault();
    }

    public static List<RecipeIngredient> ReadRecipeIngredients(GridReader reader)
    {
        var recipesIngredients = reader.Read<RecipeIngredient>().ToList();
        return recipesIngredients;
    }

    public static List<RecipeDto> ReadRecipeDtos(GridReader reader)
    {
        var recipesDto = reader.Read<RecipeDto>().ToList();
        return recipesDto;
    }
}
