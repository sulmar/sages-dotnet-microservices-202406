
var builder = WebApplication.CreateBuilder(args);

// dotnet add package AspNetCore.HealthChecks.UI.Client
// dotnet add package AspNetCore.HealthChecks.UI.SQLite.Storage

builder.Services
    .AddHealthChecksUI()
    .AddSqliteStorage(builder.Configuration.GetConnectionString("HealthCheckConnectionString"));

builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint("Catalog Api", "https://localhost:7178/api/products/hc");
    options.AddHealthCheckEndpoint("Cart Api", "https://localhost:7036/api/cart/hc");
});

var app = builder.Build();

app.MapHealthChecksUI(options => options.UIPath = "/monitor");

app.Run();
