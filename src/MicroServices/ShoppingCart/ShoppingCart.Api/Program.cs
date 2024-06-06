using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Intrastructure;
using StackExchange.Redis;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // builder.Services.AddSingleton<IShoppingCartRepository, FakeShoppingCartRepository>();

        builder.Services.AddTransient<IShoppingCartRepository, RedisShoppingCartRepository>();
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(new ConfigurationOptions()
        {
            EndPoints = { "localhost:6379" }
        }));


        builder.Services.AddSingleton<Context>();

        builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
        {
            // policy.AllowAnyOrigin();
            policy.WithOrigins("https://localhost:7108");
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        }));


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseCors();
        }

        app.MapGet("/", () => "Hello Shopping Cart!");

        app.MapGet("/ping", () => Results.Ok("pong"));

        app.MapPost("api/cart", (Product product, IShoppingCartRepository repository) =>
        {
            repository.Add(product);

            return Results.Ok();
        });

        app.Run();
    }
}