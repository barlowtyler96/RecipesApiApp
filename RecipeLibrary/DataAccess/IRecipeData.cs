using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface IRecipeData
    {
        Task Delete(int recipesId);
        Task<List<RecipeDto>> GetAll();
        Task UpdateAllColumns(int recipeId, RecipeDto recipeDto);
        Task<PaginationResponse<List<RecipeDto>>> GetByDate(int currentPageNumber, int pageSize);
        Task<List<RecipeModel>> GetById(int id);
        Task<PaginationResponse<List<RecipeDto>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize);
    }
}