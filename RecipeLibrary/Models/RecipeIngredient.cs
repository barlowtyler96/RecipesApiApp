using System.Text.Json.Serialization;

namespace RecipeLibrary.Models;

public class RecipeIngredient
{
    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public decimal Amount { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Unit { get; set; }

    public string? IngredientName { get; set; }
}
