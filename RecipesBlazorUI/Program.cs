using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//builder.Services.AddTransient<ClientApiWithRolesService>();
builder.Services.AddHttpClient();

string[] initialScopes = builder.Configuration.GetValue<string>
        ("ApiWithRoles:ScopeForAccessToken")?.Split(' ');

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();


builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient("api", opts =>
{
    opts.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiUrl"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
