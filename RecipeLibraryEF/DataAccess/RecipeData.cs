using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;
namespace RecipeLibraryEF.DataAccess;

public class RecipeData : IRecipeData
{
    private readonly RecipeContext _context;
    private readonly IMapper _mapper;
    private readonly IUserData _userData;

    public RecipeData(RecipeContext context, IMapper mapper, IUserData userData)
    {
        _context = context;
        _mapper = mapper;
        _userData = userData;
    }
    //GET
    public async Task<RecipeDto> GetByIdAsync(int id)
    {
        var recipeResponse = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipeResponse == null)
        {
            return null!;
        }

        RecipeDto createdRecipe = _mapper.Map<RecipeDto>(recipeResponse);
        return createdRecipe;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetRecipesAsync(int currentPageNumber, int pageSize, string sub)
    {
        int totalCount = await _context.Recipes.CountAsync();

        var recipes = await _context.Recipes
            .OrderBy(r => r.Id)
            .Skip((currentPageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Instructions = r.Instructions,
                CreatedOn = r.CreatedOn,
                ImageUrl = r.ImageUrl,
                Ingredients = r.RecipeIngredients
                    .Select(ri => new IngredientDto
                    {
                        Id = ri.Ingredient.Id,
                        Name = ri.Ingredient.Name,
                        Amount = ri.Amount,
                        Unit = ri.Unit
                    }).ToList(),
                IsFavorited = r.UserFavorites.Any(uf => uf.Sub == sub)
            })
            .ToListAsync();

        PaginationResponse<List<RecipeDto>> pagedResponse = new(totalCount, pageSize, currentPageNumber, recipes);
        return pagedResponse;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetRecipesRecentAsync(int currentPageNumber, int pageSize, string sub)
    {
        int totalCount = await _context.Recipes.CountAsync();

        var recipes = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .OrderByDescending(r => r.CreatedOn)
            .Skip((currentPageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.UserFavorites)
            .ToListAsync();

        List<RecipeDto> recipeDtos = recipes.Select(recipe =>
        {
            var recipeDto = _mapper.Map<RecipeDto>(recipe);
            recipeDto.IsFavorited = recipe.UserFavorites.Any(uf => uf.Sub == sub);
            return recipeDto;
        }).ToList();

        PaginationResponse<List<RecipeDto>> pagedResponse = new(totalCount, pageSize, currentPageNumber, recipeDtos);
        return pagedResponse;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetByKeywordAsync(string keyword, int currentPageNumber, int pageSize, string sub)
    {
        keyword = keyword?.Trim() ?? string.Empty;

        var recipes = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Where(r => r.Name.Contains(keyword) || r.Description.Contains(keyword))
            .Skip((currentPageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(r => r.UserFavorites)
            .ToListAsync();

        List<RecipeDto> recipeDtos = recipes.Select(recipe =>
        {
            var recipeDto = _mapper.Map<RecipeDto>(recipe);
            recipeDto.IsFavorited = recipe.UserFavorites.Any(uf => uf.Sub == sub);
            return recipeDto;
        }).ToList();

        PaginationResponse<List<RecipeDto>> pagedResponse = new (recipes.Count , pageSize, currentPageNumber, recipeDtos);
        return pagedResponse;
    }

    //POST
    public async Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipeDto)
    {
        Recipe newRecipe = _mapper.Map<Recipe>(newRecipeDto);

        foreach (var recipeIngredient in newRecipe.RecipeIngredients)
        {
            if(recipeIngredient.Ingredient != null)
            {
                var existingIngredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name == recipeIngredient.Ingredient.Name);

                if (existingIngredient != null)
                {
                    recipeIngredient.IngredientId = existingIngredient.Id;
                    recipeIngredient.Ingredient = existingIngredient;
                }
                else
                {
                    _context.Ingredients.Add(recipeIngredient.Ingredient);
                }
            }
        }

        _context.Recipes.Add(newRecipe);
        await _context.SaveChangesAsync();
        RecipeDto createdRecipe = _mapper.Map<RecipeDto>(newRecipe);
        return createdRecipe;
    }

    //DELETE
    public async Task DeleteRecipeAsync(int id)
    {
        Recipe recipe = _context.Recipes.Find(id)!;
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();
    }
}
