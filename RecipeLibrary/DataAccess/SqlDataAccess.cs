using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace RecipeLibrary.DataAccess;

public class SqlDataAccess
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
        string connectionString = _config.GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);

        var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

    // ABOVE THIS METHOD IS THE ASYNC VERSION
    //public List<T> LoadData<T, U>(
    //    string storedProcedure,
    //    U parameters,
    //    string connectionStringName)
    //{
    //    string connectionString = _config.GetConnectionString(connectionStringName);

    //    using IDbConnection connection = new SqlConnection(connectionString);

    //    List<T> rows = connection.Query<T>(storedProcedure, parameters,
    //        commandType: CommandType.StoredProcedure).ToList();
    //    return rows;
    //}

    public Task SaveData<T>(string storedProcedure, 
        T parameters, 
        string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);

        return connection.ExecuteAsync(
            storedProcedure, 
            parameters, 
            commandType: CommandType.StoredProcedure);
    }
}
