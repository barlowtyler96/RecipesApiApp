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
    public Task<List<RecipeModel>> GetNameDescription()
    {
        return _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetNameDescription",
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
        string instructions)
    {
        var results = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_Create",
            new { Name = name, Description = description, Instructions = instructions },
            "Default");

        return results.FirstOrDefault();
    }

    //PUT
    public Task UpdateAll(int recipesId, string name, string description, string instructions)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_UpdateAll",
            new { RecipesId =  recipesId, Name = name, Description = description, Instructions = instructions},
            "Default");
    }
}
