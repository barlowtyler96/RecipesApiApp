using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);

        Task<RecipeModel> LoadRecipeModelData<T, U>(
            string storedProcedure,
            U parameters,
            string connectionStringName);

        Task<PaginationResponse<List<RecipeDto>>> LoadPaginationData<T, U>(
            string storedProcedure, 
            U parameters, 
            string connectionStringName, 
            int currentPageNumber, 
            int pageSize);

        Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
        Task<PaginationResponse<List<RecipeModel>>> LoadRecipeModelData<T, U>(
            string storedProcedure, 
            U parameters, 
            string connectionStringName,
            int currentPageNumber, int pageSize);
    }
}