using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RecipeLibraryEF.Models.Entities;

public class Recipe
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Instructions { get; set; }

    [Required]
    [MaxLength(100)]
    public string CreatedBySub { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    [MaxLength(200)]
    public string ImageUrl { get; set; }

    //Navigation property
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<RecipeIngredient> RecipeIngredients { get; set; }

    //Navigation property
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<UserFavorite> UserFavorites { get; set; }

    //Navigation property
    [ForeignKey("CreatedBySub")]
    public User CreatedBy { get; set; }
    //just make this a string?
}
