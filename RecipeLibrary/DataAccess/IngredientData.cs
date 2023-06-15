using RecipeLibrary.Models;
namespace RecipeLibrary.DataAccess;

internal class IngredientData
{
    private readonly ISqlDataAccess _sql;

    public IngredientData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    //GET
    public Task<List<IngredientModel>> GetAll()
    {
        return _sql.LoadData<IngredientModel, dynamic>(
            "dbo.spIngredients_GetAll",
            new { },
            "Default");
    }

    //GET
    public async Task<IngredientModel?> GetByName(string name)
    {
        var results = await _sql.LoadData<IngredientModel, dynamic>(
            "dbo.spIngredients_GetByName",
            new { Name = name },
            "Default");

        return results.FirstOrDefault();
    }

    //POST
    public async Task<IngredientModel?> Create(string name, string description)
    {
        var results = await _sql.LoadData<IngredientModel, dynamic>(
            "dbo.spIngredients_Create",
            new { Name = name, Description = description },
            "Default");

        return results.FirstOrDefault();
    }

    //PUT
    public Task UpdateAllColumns(int ingredientsId, string name,
        string description)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spIngredients_UpdateAll",
            new { IngredientsId = ingredientsId, Name = name, Description = description },
            "Default");
    }
}
