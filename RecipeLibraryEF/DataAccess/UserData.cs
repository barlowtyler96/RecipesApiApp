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

    public async Task AddUserFavoriteAsync(UserFavoriteDto userFavoriteDto)
    {
        //get user id based on sub
        var userId = await _context.Users
                            .Where(u => u.UserSub == userFavoriteDto.UserSub)
                            .Select(u => u.Id)
                            .FirstOrDefaultAsync();
        userFavoriteDto.UserId = userId;

        var userFavorite = _mapper.Map<UserFavorite>(userFavoriteDto);
        _context.UserFavorites.Add(userFavorite);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserFavoriteAsync(UserFavoriteDto userFavoriteDto)
    {
        //get user id based on sub
        var userId = await _context.Users
                            .Where(u => u.UserSub == userFavoriteDto.UserSub)
                            .Select(u => u.Id)
                            .FirstOrDefaultAsync();
        userFavoriteDto.UserId = userId;

        var userFavorite = _mapper.Map<UserFavorite>(userFavoriteDto);
        _context.UserFavorites.Remove(userFavorite);
        await _context.SaveChangesAsync();
    }

    public async Task<List<RecipeDto>> GetUserFavoriteRecipesAsync(string userSub)
    {
        var favoriteRecipes = await _context.UserFavorites
            .Where(uf => uf.User.UserSub == userSub)
            .Select(uf => uf.Recipe)
            .ToListAsync();

        var favoriteRecipeDtos = _mapper.Map<List<RecipeDto>>(favoriteRecipes);
        return favoriteRecipeDtos;
    }

    public async Task<List<int>> GetUserFavoriteIdsAsync(string userSub)
    {
        var favoriteRecipes = await _context.UserFavorites
            .Where(uf => uf.User.UserSub == userSub)
            .Select(uf => uf.RecipeId)
            .ToListAsync();
        return favoriteRecipes;
    }

    public async Task AddNewUserAsync(User newUser)
    {
        bool userExists = await _context.Users
            .AnyAsync(u => u.UserSub == newUser.UserSub);

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
