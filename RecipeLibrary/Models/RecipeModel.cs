
namespace RecipeLibrary.Models;

public class RecipeModel    
{
    public int RecipeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    public string? ImagePath { get; set; }

    public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

}
