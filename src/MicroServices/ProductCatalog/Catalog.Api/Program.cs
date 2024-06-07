using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities;
using Catalog.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck("Ping", () => HealthCheckResult.Healthy())
    .AddCheck("Random", () =>
    {
        if (DateTime.Now.Minute % 2 == 0)
            return HealthCheckResult.Healthy();
        else
            return HealthCheckResult.Unhealthy();
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello Catalog Api!");

app.MapGet("api/products", async (IProductRepository repository) => Results.Ok(await repository.GetAllAsync()));

app.MapGet("api/products/ping", () => Results.Ok("pong"));

app.MapHealthChecks("api/products/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    // dotnet add package AspNetCore.HealthChecks.UI.Client
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.Run();
