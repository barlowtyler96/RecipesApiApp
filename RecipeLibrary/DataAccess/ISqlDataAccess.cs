using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task<PaginationResponse<List<RecipeDto>>> LoadMultiData<T, U>(string storedProcedure, U parameters, string connectionStringName, int currentPageNumber, int pageSize);
        Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}