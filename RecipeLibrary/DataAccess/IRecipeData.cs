using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface IRecipeData
    {
        Task Delete(int recipesId);
        Task<List<RecipeDto>> GetAll();
        Task UpdateAllColumns(int recipeId, RecipeDto recipeDto);
        Task<List<RecipeModel>> GetByDate();
        Task<List<RecipeModel>> GetById(int id);
        Task<int> Create(RecipeModel recipeModel);
        Task<PaginationResponse<List<RecipeModel>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize);
    }
}