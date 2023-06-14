using RecipeLibrary.Models;

namespace RecipeLibrary.DataAccess;

public class RecipeData
{
    private readonly ISqlDataAccess _sql;

    public RecipeData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<RecipeModel>> GetNameDescription()
    {
        return _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetNameDescription",
            new { },
            "Default");
    }
    
    public Task<List<RecipeModel>> GetByName(string name)
    {
        return _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetByName",
            new { Name = name},
            "Default");
    }
}
