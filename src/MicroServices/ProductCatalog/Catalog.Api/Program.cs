using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities;
using Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IProductRepository, FakeProductRepository>();
builder.Services.AddSingleton<Context>(_ =>
{
    var products = new List<Product>
    {
        new() { Id = 1, Name = "Product 1", Price = 1.99m },
        new() { Id = 2, Name = "Product 2", Price = 2.99m },
        new() { Id = 3, Name = "Product 3", Price = 3.99m },
        new() { Id = 4, Name = "Product 4", Price = 4.99m },
        new() { Id = 5, Name = "Product 5", Price = 5.99m },
    };

    return new Context { Products = products.ToDictionary(p => p.Id) };
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    // policy.AllowAnyOrigin();
    policy.WithOrigins("https://localhost:7108");
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello Catalog Api!");

app.MapGet("api/products", async (IProductRepository repository) => Results.Ok(await repository.GetAllAsync()));

app.MapGet("/ping", () => Results.Ok("pong"));

app.Run();
