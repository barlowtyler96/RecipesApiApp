namespace RecipeLibrary.DataAccess;

public interface ISqlDataAccess
{
    Task<List<T>> LoadData<T, U>(
        string storedProcedure, 
        U parameters, 
        string connectionStringName);


    Task<PaginationResponse<List<T>>> LoadPaginationData<T, U>(
        string storedProcedure, 
        U parameters, 
        string connectionStringName,
        int currentPageNumber,
        int pageSize);

    Task<int> SaveData<T>(
        string storedProcedure, 
        T parameters, 
        string connectionStringName);
}