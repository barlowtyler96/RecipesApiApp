using System.Text.Json.Serialization;

namespace RecipeLibraryEF.Models.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Unit { get; set; }
}