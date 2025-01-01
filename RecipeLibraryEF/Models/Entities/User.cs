using System.ComponentModel.DataAnnotations;
namespace RecipeLibraryEF.Models.Entities;

public class User
{
    [MaxLength(100)]
    [Required]
    [Key]
    public string Sub { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    [Required]
    public string LastName { get; set; }
    public List<UserFavorite> UserFavorites { get; set; }
}
