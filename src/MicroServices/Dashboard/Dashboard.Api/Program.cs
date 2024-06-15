using Catalog.Domain.Entities;
using Dashboard.Api.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("productservice", client =>
{
    client.BaseAddress = new Uri("https://localhost:7178"); // Adres Product Catalog Service
});

builder.Services.AddHttpClient("orderingservice", client =>
{
    client.BaseAddress = new Uri("https://localhost:7065"); // Adres Ordering Service
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello Dashboard.Api!");

app.MapGet("api/dashboard", async (IHttpClientFactory httpClientFactory) =>
{    
    var productClient = httpClientFactory.CreateClient("productservice");
    var orderingClient = httpClientFactory.CreateClient("orderingservice");


    var productResponse = await productClient.GetAsync($"/api/products");

    var products = await productResponse.Content.ReadFromJsonAsync<List<Product>>();

    var orderResponse = await orderingClient.GetAsync("/api/orders");

    var orders = await orderResponse.Content.ReadFromJsonAsync<List<Order>>();

    // Aggregate data
    var aggregatedMetrics = new AggregatedMetrics
    {
         TotalSales = products.Count(),
         NewUsers = orders.Count(),
    };

    return aggregatedMetrics;
});

app.Run();
