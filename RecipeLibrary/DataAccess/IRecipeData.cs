using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess;

public interface IRecipeData
{
    Task<PaginationResponse<List<RecipeDto>>> GetAllRecipeDtos(int currentPageNumber, int pageSize);
    Task<PaginationResponse<List<RecipeModel>>> GetAllRecipeModels(int currentPageNumber, int pageSize);
    Task<PaginationResponse<List<RecipeDto>>> GetByDate(int currentPageNumber, int pageSize);
    Task<RecipeModel> GetById(int id);
    Task<PaginationResponse<List<RecipeDto>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize);
}