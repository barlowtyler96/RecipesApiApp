using Dapper;
using RecipeLibrary.Models;
using System.Data;

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
    public async Task<List<RecipeDto>> GetById(int id)
    {
        var recipes = await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spGetRecipeById",
            new 
            { RecipeId = id},
            "Default");

        var recipeId = recipes[0].RecipeId;
        var recipeIdsAsString = string.Join(",", recipeId);
        var recipeIngredients = await _sql.LoadData<RecipeIngredient, dynamic>(
            "dbo.spGetRecipeIngredientsById",
            new { RecipeIdsAsString = recipeIdsAsString },
            "Default");

        var ingredientIdsAsString = string.Join(",", recipeIngredients.Select(i => i.IngredientId));
        var ingredients = await _sql.LoadData<IngredientModel, dynamic>(
            "dbo.spGetIngredientsById",
            new { IngredientIdsAsString = ingredientIdsAsString },
            "Default");

        for (int i = 0; i < ingredients.Count && i < recipeIngredients.Count; i++)
        {
            recipeIngredients[i].IngredientName = ingredients[i].Name;
        }

        List<RecipeDto> recipeDtos = recipes.Select(recipe => new RecipeDto
        {
            Id = recipe.RecipeId,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            ImagePath = recipe.ImagePath,
            RecipeIngredients = recipeIngredients.Where(ri => ri.RecipeId == recipe.RecipeId).ToList()
        }).ToList();

        return recipeDtos;
    }

    //GET
    public async Task<List<RecipeDto>> GetByDate()
    {
        var recipes =  await _sql.LoadData<RecipeModel, dynamic>(
            "dbo.spGetRecipesByDate",
            new { },
            "Default");
        var recipeIds = new List<int>();

        foreach (var r in recipes)
        {
            recipeIds.Add(r.RecipeId);
        }

        var recipeIdsAsString = string.Join(",", recipeIds);
        var recipeIngredients = await _sql.LoadData<RecipeIngredient, dynamic>(
            "dbo.spGetRecipeIngredientsById",
            new { RecipeIdsAsString = recipeIdsAsString },
            "Default");

        var ingredientIdsAsString = string.Join(",", recipeIngredients.Select(i => i.IngredientId));
        var ingredients = await _sql.LoadData<IngredientModel, dynamic>(
            "dbo.spGetIngredientsById",
            new { IngredientIdsAsString = ingredientIdsAsString },
            "Default");

        for (int i = 0; i < ingredients.Count && i < recipeIngredients.Count; i++)
        {
            recipeIngredients[i].IngredientName = ingredients[i].Name;
        }

        List<RecipeDto> recipeDtos = recipes.Select(recipe => new RecipeDto
        {
            Id = recipe.RecipeId,
            Name = recipe.Name,
            Description = recipe.Description,
            Instructions = recipe.Instructions,
            ImagePath = recipe.ImagePath,
            RecipeIngredients = recipeIngredients.Where(ri => ri.RecipeId == recipe.RecipeId).ToList()
        }).ToList();

        return recipeDtos;
    }

    public async Task<PaginationResponse<List<RecipeModel>>> GetByKeyword(string keyword, int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        var results = await _sql.LoadMultiData<PaginationResponse<RecipeModel>, dynamic>(
            "dbo.spRecipes_GetByKeyword",
            new { Keyword = keyword, Skip = skip, Take = take },
            "Default",
            currentPageNumber,
            pageSize);
        return results;
    }

    //POST
    public async Task<int> Create(RecipeDto recipeDto)
    {
        var recipeIngredientsTable = new DataTable();
        recipeIngredientsTable.Columns.Add("IngredientName", typeof(string));
        recipeIngredientsTable.Columns.Add("Amount", typeof(decimal));
        recipeIngredientsTable.Columns.Add("Unit", typeof(string));

        foreach (var ingredient in recipeDto.RecipeIngredients)
        {
            recipeIngredientsTable.Rows.Add(ingredient.IngredientName, ingredient.Amount, ingredient.Unit);
        }

        var createdRecipeId = await _sql.LoadData<int, dynamic>(
            "spTestReview_InsertRecipeWithIngredients",
            new
            {
                recipeDto.Name,
                recipeDto.Description,
                recipeDto.Instructions,
                recipeDto.ImagePath,
                RecipeIngredients = recipeIngredientsTable.AsTableValuedParameter("RecipeIngredientType")
            },
            "Default");
        return createdRecipeId.FirstOrDefault();
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
