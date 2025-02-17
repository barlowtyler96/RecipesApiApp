﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public async Task<RecipeDto> GetByIdAsync(int id, string sub)
    {
        var recipeDto = await _context.Recipes
            .Where(r => r.Id == id)
            .Select(r => new RecipeDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Instructions = r.Instructions,
                CreatedOn = r.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = r.ImageUrl,
                CreatedBy = new UserDto
                {
                    FirstName = r.CreatedBy.FirstName,
                    LastName = r.CreatedBy.LastName
                },
                Ingredients = r.RecipeIngredients
                    .Select(ri => new IngredientDto
                    {
                        Id = ri.Ingredient.Id,
                        Name = ri.Ingredient.Name,
                        Amount = ri.Amount,
                        Unit = string.IsNullOrWhiteSpace(ri.Unit) ? null : ri.Unit
                    }).ToList(),
                IsFavorited = r.UserFavorites.Any(uf => uf.Sub == sub)
            })
            .FirstOrDefaultAsync();

        return recipeDto;
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
                CreatedOn = r.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = r.ImageUrl,
                CreatedBy = new UserDto
                {
                    FirstName = r.CreatedBy.FirstName,
                    LastName = r.CreatedBy.LastName
                },
                Ingredients = r.RecipeIngredients
                    .Select(ri => new IngredientDto
                    {
                        Id = ri.Ingredient.Id,
                        Name = ri.Ingredient.Name,
                        Amount = ri.Amount,
                        Unit = string.IsNullOrWhiteSpace(ri.Unit) ? null : ri.Unit
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
            .OrderByDescending(r => r.CreatedOn)
            .Skip((currentPageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Instructions = r.Instructions,
                CreatedOn = r.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = r.ImageUrl,
                CreatedBy = new UserDto
                {
                    FirstName = r.CreatedBy.FirstName,
                    LastName = r.CreatedBy.LastName
                },
                Ingredients = r.RecipeIngredients
                    .Select(ri => new IngredientDto
                    {
                        Id = ri.Ingredient.Id,
                        Name = ri.Ingredient.Name,
                        Amount = ri.Amount,
                        Unit = string.IsNullOrWhiteSpace(ri.Unit) ? null : ri.Unit
                    }).ToList(),
                IsFavorited = r.UserFavorites.Any(uf => uf.Sub == sub)
            })
            .ToListAsync();
        
        PaginationResponse<List<RecipeDto>> pagedResponse = new(totalCount, pageSize, currentPageNumber, recipes);
        return pagedResponse;
    }

    public async Task<PaginationResponse<List<RecipeDto>>> GetByKeywordAsync(string keyword, int currentPageNumber, int pageSize, string sub)
    {
        keyword = keyword?.Trim() ?? string.Empty;

        var recipes = await _context.Recipes
            .Where(r => r.Name.Contains(keyword) || r.Description.Contains(keyword))
            .OrderByDescending(r => r.CreatedOn)
            .Skip((currentPageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RecipeDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Instructions = r.Instructions,
                CreatedOn = r.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = r.ImageUrl,
                CreatedBy = new UserDto
                {
                    FirstName = r.CreatedBy.FirstName,
                    LastName = r.CreatedBy.LastName
                },
                Ingredients = r.RecipeIngredients
                    .Select(ri => new IngredientDto
                    {
                        Id = ri.Ingredient.Id,
                        Name = ri.Ingredient.Name,
                        Amount = ri.Amount,
                        Unit = string.IsNullOrWhiteSpace(ri.Unit) ? null : ri.Unit
                    }).ToList(),
                IsFavorited = r.UserFavorites.Any(uf => uf.Sub == sub)
            })
            .ToListAsync();

        PaginationResponse<List<RecipeDto>> pagedResponse = new(recipes.Count, pageSize, currentPageNumber, recipes);
        return pagedResponse;
    }

    //POST
    public async Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipeDto, string sub)
    {
        Recipe newRecipe = _mapper.Map<Recipe>(newRecipeDto);
        newRecipe.CreatedOn = DateTime.UtcNow;
        newRecipe.CreatedBySub = sub;

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

        var user = await _context.Users
            .Where(u => u.Sub == newRecipe.CreatedBySub)
            .Select(u => new UserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName
            })
            .FirstOrDefaultAsync(); 

        createdRecipe.CreatedBy = user;
        return createdRecipe;
    }

    //DELETE
    public async Task DeleteRecipeAsync(int id)
    {
        var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            // Optionally handle the case where the recipe isn't found
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();


    }
}
