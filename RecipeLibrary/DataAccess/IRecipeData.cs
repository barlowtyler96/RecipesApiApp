using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface IRecipeData
    {
        Task Delete(int recipesId);
        Task<List<RecipeModel>> GetAll();
        Task UpdateAllColumns(int recipeId, RecipeModel recipeModel);
        Task<PaginationResponse<List<RecipeModel>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize);
        Task<List<RecipeDto>> GetByDate();
        Task<List<RecipeDto>> GetById(int id);
        Task<int> Create(RecipeDto recipeDto);
    }
}