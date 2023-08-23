namespace RecipeLibrary.Models;

public class RecipeModel
{
    public int Id { get; set; }

    public string? Name { get; set; }    

    public string? Description { get; set; }

    public string? Ingredients { get; set; }

    public string? Instructions { get; set; }

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    public string? ImagePath { get; set; }
}
