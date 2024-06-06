var builder = WebApplication.CreateBuilder(args);

// dotnet add package Yarp.ReverseProxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapGet("/", () => "Hello API Gateway!");

app.MapReverseProxy();

app.Run();
