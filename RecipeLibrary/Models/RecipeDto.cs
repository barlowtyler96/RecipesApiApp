using System.Text.Json.Serialization;

namespace RecipeLibrary.Models;

public class RecipeDto
{
    public int RecipeId { get; set; }

    public string? Name { get; set; }    

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CreatedBy { get; set; }

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    public string? ImageUrl { get; set; }
}
