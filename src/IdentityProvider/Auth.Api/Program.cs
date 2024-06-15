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

        context.Response.Cookies.Append("access-token", accessToken, new CookieOptions
        {
            HttpOnly = true, // Zabezpieczenie przed dost�pem przez JavaScript (np. przez document.cookie)
            Secure = true,   // Wymusza, aby plik cookie by� wysy�any tylko przez bezpieczne po��czenia HTTPS.
            Expires = DateTimeOffset.Now.AddMinutes(30), // Ustawia czas wyga�ni�cia pliku cookie. Po up�ywie tego czasu plik cookie zostanie automatycznie usuni�ty przez przegl�dark�.
        });

        return Results.Ok(accessToken);
    }

    return Results.Unauthorized();
});

app.Run();
