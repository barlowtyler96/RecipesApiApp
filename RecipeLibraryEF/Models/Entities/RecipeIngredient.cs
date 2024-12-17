using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeLibraryEF.Models.Entities;

public class RecipeIngredient
{
    [Required]
    public int RecipeId { get; set; }

    [Required]
    public int IngredientId { get; set; }

    //Navigation property
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Recipe? Recipe { get; set; }

    //Navigation property
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Ingredient? Ingredient { get; set; }

    [Required]
    public double Amount { get; set; }

    [MaxLength(100)]
    public string? Unit { get; set; }

}