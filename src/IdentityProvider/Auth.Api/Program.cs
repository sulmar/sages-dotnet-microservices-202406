using Auth.Api.Abstractions;
using Auth.Api.Infrastructure;
using Auth.Api.Model;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserIdentityRepository, FakeUserIdentityRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddScoped<IPasswordHasher<UserIdentity>, PasswordHasher<UserIdentity>>();

var app = builder.Build();

app.MapGet("/", () => "Hello Auth.Api!");

app.MapPost("api/login", async (LoginModel model,
    IAuthService authService,
    ITokenService tokenService,
    HttpContext context) =>
{
    var result = await authService.AuthorizeAsync(model.Username, model.Password);

    if (result.IsAuthorized)
    {
        var accessToken = tokenService.CreateAccessToken(result.Identity);

        context.Response.Cookies.Append("jwt-token", accessToken, new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(15) });

        return Results.Ok(accessToken);
    }

    return Results.Unauthorized();
});

app.Run();
