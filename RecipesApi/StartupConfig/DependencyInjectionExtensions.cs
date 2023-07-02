using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using RecipeLibrary.DataAccess;
using RecipesApi.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.Identity.Web;

namespace RecipesApi.StartupConfig;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<IRecipeData, RecipeData>();
    }

    public static void AddHealthCheckServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("Default"));
    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        
    }
}
