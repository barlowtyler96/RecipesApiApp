using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess;

public class RecipeData
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
    public async Task<RecipeModel?> GetByName(string name)
    {
        var results = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetByName",
            new { Name = name},
            "Default");

        return results.FirstOrDefault();
    }

    //POST
    public async Task<RecipeModel?> Create(string name, 
        string description, 
        string instructions,
        string ingredients)
    {
        var results = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_Create",
            new { Name = name, Description = description, 
                  Instructions = instructions, Ingredients = ingredients },
            "Default");

        return results.FirstOrDefault();
    }

    //PUT
    public Task UpdateAllColumns(int recipesId, string name, 
        string description, string instructions, string ingredients)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_UpdateAll",
            new { RecipesId =  recipesId, Name = name, 
                  Description = description, Instructions = instructions, Ingredients = ingredients },
            "Default");
    }
}
