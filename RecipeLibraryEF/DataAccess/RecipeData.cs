﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;
namespace RecipeLibraryEF.DataAccess;

public class RecipeData : IRecipeData
{
    private readonly RecipeContext _context;
    private readonly IMapper _mapper;

    public RecipeData(RecipeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
    public async Task<PaginationResponse<List<RecipeDto>>> GetAllRecipesAsync(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;
        int take = pageSize;

        int totalCount = await _context.Recipes.CountAsync();
        List<Recipe> recipesResponse = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .OrderBy(r => r.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        List<RecipeDto> recipeDtos = _mapper.Map<List<RecipeDto>>(recipesResponse);
        PaginationResponse<List<RecipeDto>> pagedResponse = new(totalCount, pageSize, currentPageNumber, recipeDtos);
        return pagedResponse;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetByDateAsync(int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;

        int totalCount = await _context.Recipes.CountAsync();
        List<Recipe> recipesResponse = await _context.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .OrderBy(r => r.CreatedOn)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        List<RecipeDto> recipeDtos = _mapper.Map<List<RecipeDto>>(recipesResponse);
        PaginationResponse<List<RecipeDto>> pagedResponse = new(totalCount, pageSize, currentPageNumber, recipeDtos);
        return pagedResponse;
    }

    //GET
    public async Task<PaginationResponse<List<RecipeDto>>> GetByKeywordAsync(string keyword, int currentPageNumber, int pageSize)
    {
        int skip = (currentPageNumber - 1) * pageSize;

        int totalCount = await _context.Recipes
                                   .Where(r => r.Name.Contains(keyword) ||
                                               r.Description.Contains(keyword))
                                   .CountAsync();


        var recipes = await _context.Recipes
                                 .Include(r => r.RecipeIngredients)
                                    .ThenInclude(ri => ri.Ingredient)
                                 .Where(r => r.Name.Contains(keyword) ||
                                             r.Description.Contains(keyword))
                                 .OrderBy(r => r.Name)
                                 .Skip(skip)
                                 .Take(pageSize)
                                 .ToListAsync();

        var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);
        PaginationResponse<List<RecipeDto>> pagedResponse = new(totalCount, pageSize, currentPageNumber, recipeDtos);
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
                // Check if the ingredient already exists
                var existingIngredient = await _context.Ingredients
                    .FirstOrDefaultAsync(i => i.Name == recipeIngredient.Ingredient.Name);

                if (existingIngredient != null)
                {
                    // Use the existing ingredient
                    recipeIngredient.IngredientId = existingIngredient.Id;
                    recipeIngredient.Ingredient = existingIngredient;
                }
                else
                {
                    // Add the new ingredient to the context
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
