using RecipeLibraryEF.Models.Dtos;

namespace RecipeLibraryEF.DataAccess;

public interface IRecipeData
{
    Task<PaginationResponse<List<RecipeDto>>> GetAllRecipesAsync(int currentPageNumber, int pageSize);
    Task<PaginationResponse<List<RecipeDto>>> GetByDateAsync(int currentPageNumber, int pageSize);
    Task<RecipeDto> GetByIdAsync(int id);
    Task<PaginationResponse<List<RecipeDto>>> GetByKeywordAsync(string keyword, int currentPageNumber, int pageSize);
    Task<RecipeDto> AddRecipeAsync(RecipeDto newRecipeDto);
    Task DeleteRecipeAsync(int id);
}