
using System.Text.Json.Serialization;

namespace RecipeLibrary.Models;

public class RecipeModel    
{
    public int RecipeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CreatedBy { get; set; }   

    public string? ImageUrl { get; set; }

    public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public RecipeModel(RecipeDto recipesDto, List<RecipeIngredient> recipeIngredients)
    {
        RecipeId = recipesDto.RecipeId;
        Name = recipesDto.Name;
        Description = recipesDto.Description;
        Instructions = recipesDto.Instructions;
        CreatedBy = recipesDto.CreatedBy;
        ImageUrl = recipesDto.ImageUrl;
        RecipeIngredients = recipeIngredients.Where(ri => ri.RecipeId == recipesDto.RecipeId).ToList();
    }
    public RecipeModel()
    {
    }
}
