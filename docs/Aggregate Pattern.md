```cs
var builder = WebApplication.CreateBuilder(args);

// Register HTTP clients
builder.Services.AddHttpClient("orderservice", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001"); // URL of OrderService
});
builder.Services.AddHttpClient("productservice", client =>
{
    client.BaseAddress = new Uri("http://localhost:5002"); // URL of ProductService
});

var app = builder.Build();

app.MapGet("/api/aggregation/order-details/{orderId}", async (int orderId, IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    // Fetch the order
    var orderResponse = await client.GetAsync($"http://orderservice/api/order/{orderId}");
    if (!orderResponse.IsSuccessStatusCode)
        return Results.NotFound();

    var order = await orderResponse.Content.ReadFromJsonAsync<Order>();

    // Fetch the product details
    var productResponse = await client.GetAsync($"http://productservice/api/product/{order.ProductId}");
    if (!productResponse.IsSuccessStatusCode)
        return Results.NotFound();

    var product = await productResponse.Content.ReadFromJsonAsync<Product>();

    // Aggregate data
    var aggregatedOrder = new AggregatedOrder
    {
        OrderId = order.Id,
        Product = product,
        Quantity = order.Quantity
    };

    return Results.Ok(aggregatedOrder);
});

app.Run();

public record Product(int Id, string Name, decimal Price);
public record Order(int Id, int ProductId, int Quantity);
public record AggregatedOrder
{
    public int OrderId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}

```