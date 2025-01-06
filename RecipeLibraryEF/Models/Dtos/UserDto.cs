using System.Text.Json.Serialization;

namespace RecipeLibraryEF.Models.Dtos;

public class UserDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Sub { get; set; } = null!;
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
