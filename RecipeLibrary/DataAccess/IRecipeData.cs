using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface IRecipeData
    {
        Task<RecipeModel?> Create(RecipeModel recipeModel);
        Task Delete(int recipesId);
        Task<List<RecipeModel>> GetAll();
        Task<RecipeModel?> GetById(int id);
        Task UpdateAllColumns(int recipeId, string name, string description, string instructions, string ingredients);
    }
}