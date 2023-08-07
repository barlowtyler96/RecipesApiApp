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

    public async Task<PaginationResponse<List<RecipeModel>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        var results = await _sql.LoadMultiData<PaginationResponse<RecipeModel>, dynamic>(
            "dbo.spRecipes_GetByKeyword",
            new { Keyword = keyword, Skip = skip, Take = take}, 
            "Default",
            currentPageNumber,
            pageSize);
        return results;
    }

    //GET
    public async Task<List<RecipeModel>> GetByDate()
    {
        return await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spRecipes_GetByDate",
            new { },
            "Default");
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
                Instructions = recipeModel.Instructions,
                DateAdded = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ImagePath = recipeModel.ImagePath
            },
            "Default");

        return results.FirstOrDefault();
    }

    //PUT
    public Task UpdateAllColumns(int recipesId, RecipeModel recipeModel)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spRecipes_UpdateAllColumns",
            new
            {
                RecipesId = recipesId,
                Name = recipeModel.Name,
                Description = recipeModel.Description,
                Ingredients = recipeModel.Ingredients,
                Instructions = recipeModel.Instructions,
                ImagePath = recipeModel.ImagePath
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
