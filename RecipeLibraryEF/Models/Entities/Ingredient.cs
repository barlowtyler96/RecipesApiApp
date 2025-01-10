using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecipeLibraryEF.Models.Entities;

public class Ingredient
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";

    //Navigation property
    public List<RecipeIngredient> RecipeIngredients { get; set; }

}