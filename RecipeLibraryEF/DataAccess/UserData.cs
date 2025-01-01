using AutoMapper;
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

    public async Task<List<RecipeDto>> GetUserFavoriteRecipesAsync(string userSub)
    {
        var favoriteRecipes = await _context.UserFavorites
            .Where(uf => uf.User.Sub == userSub)
            .Select(uf => uf.Recipe)
            .ToListAsync();

        var favoriteRecipeDtos = _mapper.Map<List<RecipeDto>>(favoriteRecipes);
        return favoriteRecipeDtos;
    }

    public async Task<List<int>> GetUserFavoriteIdsAsync(string userSub)
    {
        var favoriteRecipes = await _context.UserFavorites
            .Where(uf => uf.User.Sub == userSub)
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
