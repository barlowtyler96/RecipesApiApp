using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;
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

    public async Task<List<RecipeDto>> GetUserFavoriteRecipesAsync(string sub)
    {
        var favoriteRecipeDtos = await _context.UserFavorites
            .Where(uf => uf.Sub == sub)
            .Select(uf => new RecipeDto
            {
                Id = uf.Recipe.Id,
                Name = uf.Recipe.Name,
                Description = uf.Recipe.Description,
                Instructions = uf.Recipe.Instructions,
                CreatedOn = uf.Recipe.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = uf.Recipe.ImageUrl,
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

        return favoriteRecipeDtos;
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

    public async Task<List<RecipeDto>> GetUserCreatedRecipesAsync(string userSub)
    {
        throw new NotImplementedException();
    }
}
