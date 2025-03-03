using GlobalAzure.NetAspire.Server.Data;
using GlobalAzure.NetAspire.Server.Data.Entities;
using GlobalAzure.NetAspire.Server.Extensions;
using GlobalAzure.NetAspire.Server.Interfaces;
using GlobalAzure.NetAspire.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisDistributedCache("cache");

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies()
    .ApplicationCookie!.Configure(opt => opt.Events = new CookieAuthenticationEvents()
    {
        OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    });
builder.Services.AddAuthorizationBuilder();

builder.Services.AddSingleton<IUserValidatorClient, UserValidatorClient>();
builder.Services.AddHttpClient("UserValidatorClient", httpClient =>
{
    httpClient.BaseAddress = new("https+http://aspiredemoapi");
});

builder.AddSqlServerDbContext<ApplicationDbContext>("aspiredemodb");

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapIdentityApi<ApplicationUser>();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.SeedData();
}

app.UseHttpsRedirection();

// protection from cross-site request forgery (CSRF/XSRF) attacks with empty body
// form can't post anything useful so the body is null, the JSON call can pass
// an empty object {} but doesn't allow cross-site due to CORS.
app.MapPost("/logout", async (
    SignInManager<ApplicationUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty is not null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.NotFound();
}).RequireAuthorization();

app.MapFallbackToFile("/index.html");

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
