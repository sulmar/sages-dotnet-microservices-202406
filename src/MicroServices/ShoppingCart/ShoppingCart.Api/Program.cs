using MassTransit;
using Shared.Models;
using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Intrastructure;
using StackExchange.Redis;
using Shared.Messaging;
using System.Reflection;
using HealthChecks.UI.Client;
using RabbitMQ.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

        builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        var rabbitMQConnectionString = builder.Configuration.GetConnectionString("RabbitMQ");

        builder.Services.AddHealthChecks()
            .AddCheck("Ping", () => HealthCheckResult.Healthy())
            .AddRabbitMQ(rabbitConnectionString: rabbitMQConnectionString, name: "RabbitMQ");

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseCors();
        }


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello Shopping Cart!");

        app.MapGet("/ping", () => Results.Ok("pong"));

        app.MapPost("api/cart", (Product product, IShoppingCartRepository repository) =>
        {
            repository.Add(product);

            return Results.Ok();
        });

        app.MapPost("api/cart/submit", async (IShoppingCartRepository repository, IPublishEndpoint endpoint, HttpContext context) =>
        {
            var emailClaim = context.User.Claims.FirstOrDefault(c => c.Type == "email");

            // TODO: Get cart from repository

            Product product1 = new Product { Id = 1, Name = "Product 1", Price = 1.90m };
            Product product2 = new Product { Id = 2, Name = "Product 2", Price = 2.90m };

            Cart cart = new Cart();
            cart.Add(new CartItem { Product = product1, Quantity = 1 });
            cart.Add(new CartItem { Product = product2, Quantity = 2 });

            var order = new OrderDTO();

            order.Details = cart.Items.Select(p =>
            new OrderDetail { ProductId = p.Product.Id, Quantity = p.Quantity });

            var message = new SubmitOrder(order);

            await endpoint.Publish(message);

            return Results.Accepted();

        });

        app.MapHealthChecks("api/cart/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            // dotnet add package AspNetCore.HealthChecks.UI.Client
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.Run();
    }
}