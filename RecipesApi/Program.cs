using RecipesApi.StartupConfig;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.AddStandardServices();
builder.AddAuthServices();
builder.AddHealthCheckServices();
builder.AddCustomServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
