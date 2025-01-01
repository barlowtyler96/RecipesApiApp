namespace RecipeLibraryEF.Models.Dtos;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Instructions { get; set; }
    public string CreatedBySub { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ImageUrl { get; set; }
    public List<IngredientDto> Ingredients { get; set; }
    public bool IsFavorited { get; set; } = false;
}
