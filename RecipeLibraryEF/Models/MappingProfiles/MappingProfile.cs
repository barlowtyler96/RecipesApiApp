using AutoMapper;
using RecipeLibraryEF.Models.Dtos;
using RecipeLibraryEF.Models.Entities;
using System.Globalization;

namespace RecipeLibraryEF.Models.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Recipe, RecipeDto>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients.Select(ri => new IngredientDto
            {
                Id = ri.IngredientId,
                Name = ri.Ingredient.Name,
                Amount = ri.Amount,
                Unit = ri.Unit
            }).ToList()));

        CreateMap<RecipeDto, Recipe>()
            .ForMember(dest => dest.RecipeIngredients, opt => opt.MapFrom(src => src.Ingredients.Select(ri => new RecipeIngredient
            {
                IngredientId = ri.Id,
                Amount = ri.Amount,
                Unit = ri.Unit,
                Ingredient = new Ingredient { Id = ri.Id, Name = ri.Name }
            }).ToList()));

        CreateMap<Ingredient, IngredientDto>();
        CreateMap<IngredientDto, Ingredient>();
        CreateMap<UserFavorite, UserFavoriteDto>();
        CreateMap<UserFavoriteDto, UserFavorite>();

        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();

        var config = new MapperConfiguration(cfg =>
        {
            // Register your custom converter
            cfg.CreateMap<string, DateTime>().ConvertUsing<StringToDateTimeConverter>();

            // Other mappings
            cfg.CreateMap<RecipeDto, Recipe>();
            cfg.CreateMap<Recipe, RecipeDto>();
        });
    }
}

public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
{
    public DateTime Convert(string source, DateTime destination, ResolutionContext context)
    {
        DateTime.TryParseExact(source, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);
        return result; // Or handle the conversion error appropriately, e.g., throw exception or return a default DateTime.
    }
}
