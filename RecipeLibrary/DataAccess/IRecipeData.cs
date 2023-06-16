using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess
{
    public interface IRecipeData
    {
        Task<RecipeModel?> Create(string name, string description, string instructions, string ingredients);
        Task Delete(int recipesId);
        Task<List<RecipeModel>> GetAll();
        Task<RecipeModel?> GetByName(string name);
        Task UpdateAllColumns(int recipesId, string name, string description, string instructions, string ingredients);
    }
}