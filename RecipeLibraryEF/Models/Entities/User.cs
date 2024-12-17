using System.ComponentModel.DataAnnotations;
namespace RecipeLibraryEF.Models.Entities;

public class User
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string UserSub { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    [Required]
    public string LastName { get; set; }
    public List<UserFavorite> UserFavorites { get; set; }
}
