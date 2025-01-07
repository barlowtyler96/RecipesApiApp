using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;
using System.Drawing.Printing;
namespace RecipeLibraryEF.DataAccess;

public class UserData : IUserData
{
    private readonly RecipeContext _context;
    private readonly IMapper _mapper;

    public UserData(RecipeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddUserFavoriteAsync(UserFavorite userFavorite)
    {
        _context.UserFavorites.Add(userFavorite);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserFavoriteAsync(UserFavorite userFavorite)
    {
        _context.UserFavorites.Remove(userFavorite);
        await _context.SaveChangesAsync();
    }

    public async Task<PaginationResponse<List<RecipeDto>>> GetUserFavoriteRecipesAsync(string sub, int currentPage, int pageSize)
    {
        var favoriteRecipeDtos = await _context.UserFavorites
            .Where(uf => uf.Sub == sub)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .Select(uf => new RecipeDto
            {
                Id = uf.Recipe.Id,
                Name = uf.Recipe.Name,
                Description = uf.Recipe.Description,
                Instructions = uf.Recipe.Instructions,
                CreatedOn = uf.Recipe.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = uf.Recipe.ImageUrl,
                CreatedBy = new UserDto
                {
                    FirstName = uf.Recipe.CreatedBy.FirstName,
                    LastName = uf.Recipe.CreatedBy.LastName
                },
                Ingredients = uf.Recipe.RecipeIngredients.Select(ri => new IngredientDto
                {
                    Id = ri.Ingredient.Id,
                    Name = ri.Ingredient.Name,
                    Amount = ri.Amount,
                    Unit = ri.Unit
                }).ToList(),
                IsFavorited = true // Since these are all favorites
            })
            .ToListAsync();

        PaginationResponse<List<RecipeDto>> pagedResponse = new(favoriteRecipeDtos.Count, pageSize, currentPage, favoriteRecipeDtos);
        return pagedResponse;
    }

    public async Task<List<int>> GetUserFavoriteIdsAsync(string sub)
    {
        var favoriteRecipes = await _context.UserFavorites
            .Where(uf => uf.User.Sub == sub)
            .Select(uf => uf.RecipeId)
            .ToListAsync();
        return favoriteRecipes;
    }

    public async Task AddNewUserAsync(User newUser)
    {
        bool userExists = await _context.Users
            .AnyAsync(u => u.Sub == newUser.Sub);

        if (!userExists)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("User already exists.");
        }
    }

    public async Task<PaginationResponse<List<RecipeDto>>> GetUserCreatedRecipesAsync(string sub, int currentPage, int pageSize)
    {
        var userCreatedRecipes = await _context.Recipes
            .Where(r => r.CreatedBy.Sub == sub)
            .Skip((currentPage - 1) * pageSize)
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

        PaginationResponse<List<RecipeDto>> pagedResponse = new(userCreatedRecipes.Count, pageSize, currentPage, userCreatedRecipes);
        return pagedResponse;
    }
}
