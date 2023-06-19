using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess;

public class RecipeData : IRecipeData
{
    private readonly ISqlDataAccess _sql;

    public RecipeData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    //GET
    public Task<List<RecipeModel>> GetAll()
    {
        return _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetAll",
            new { },
            "Default");
    }

    //GET
    public async Task<RecipeModel?> GetById(int id)
    {
        var results = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetById",
            new { Id = id },
            "Default");

        return results.FirstOrDefault();
    }

    //POST
    public async Task<RecipeModel?> Create(RecipeModel recipeModel)
    {
        var results = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_Create",
            new
            {
                Name = recipeModel.Name,
                Description = recipeModel.Description,
                Ingredients = recipeModel.Ingredients,
                Instructions = recipeModel.Instructions
            },
            "Default");

        return results.FirstOrDefault();
    }

    //PUT
    public Task UpdateAllColumns(int recipesId, string name,
        string description, string instructions, string ingredients)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_UpdateAllColumns",
            new
            {
                RecipesId = recipesId,
                Name = name,
                Description = description,
                Instructions = instructions,
                Ingredients = ingredients
            },
            "Default");
    }

    //DELETE
    public Task Delete(int recipesId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_Delete",
            new
            { RecipesId = recipesId },
            "Default");
    }
}
