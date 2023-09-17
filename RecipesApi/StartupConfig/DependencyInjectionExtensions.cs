using RecipeLibrary.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;
using System.Reflection;

namespace RecipesApi.StartupConfig;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddVersionedApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'VVV";
            opts.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddApiVersioning(opts =>
        {
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.DefaultApiVersion = new(1, 0);
            opts.ReportApiVersions = true;
        });
    }

    public static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "JWT Authorization using bearer tokens",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirements = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearerAuth"
                    }
                },
                new string[] { }
            }
        };

        builder.Services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("bearerAuth", securityScheme);
            opts.AddSecurityRequirement(securityRequirements);

            var title = "CulinaryShares API";
            var description = "Made for serving and sharing recipes";
            var contact = new OpenApiContact()
            {
                Name = "CulinaryShares Helpdesk",
                Email = "tybardesigns@gmail.com"
            };

            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"{title} v1",
                Description = description,
                Contact = contact
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
        });
    }

    public static void AddRateLimitingServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddInMemoryRateLimiting();
    }

    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<IRecipeData, RecipeData>();
        builder.Services.AddSingleton<IUserData, UserData>();
    }

    public static void AddHealthCheckServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("Default")!);
    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options);

            },
        options => { builder.Configuration.Bind("AzureAdB2C", options); });
        builder.Services.AddInMemoryTokenCaches();
    }
}
