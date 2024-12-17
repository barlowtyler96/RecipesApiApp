using AspNetCoreRateLimit;
using RecipeLibraryEF.Models.MappingProfiles;
using RecipesApi.StartupConfig;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.AddStandardServices();
builder.AddEFCoreServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.AddSwaggerServices();
builder.Services.AddMemoryCache();
builder.AddRateLimitingServices();
builder.AddAuthServices();
builder.AddCustomServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opts.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseIpRateLimiting();
app.UseCors("AllowAnyOrigin");

app.Run();
