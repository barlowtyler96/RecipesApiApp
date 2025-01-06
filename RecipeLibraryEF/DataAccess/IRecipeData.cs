using RecipeLibraryEF.Models.Dtos;

namespace RecipeLibraryEF.DataAccess;

public interface IRecipeData
{
    Task<PaginationResponse<List<RecipeDto>>> GetRecipesAsync(int currentPageNumber, int pageSize, string userSub);
    Task<PaginationResponse<List<RecipeDto>>> GetRecipesRecentAsync(int currentPageNumber, int pageSize, string userSub);
    Task<RecipeDto> GetByIdAsync(int id, string sub);
    Task<PaginationResponse<List<RecipeDto>>> GetByKeywordAsync(string keyword, int currentPageNumber, int pageSize, string userSub);
    Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipeDto);
    Task DeleteRecipeAsync(int id);
}