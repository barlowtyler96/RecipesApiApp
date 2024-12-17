namespace RecipeLibraryEF.Models.Dtos;

public class UserFavoriteDto
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public string UserSub { get; set; }
}
