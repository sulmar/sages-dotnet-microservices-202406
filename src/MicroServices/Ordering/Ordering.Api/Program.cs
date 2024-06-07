using Shared.Messaging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapGet("/", () => "Hello Ordering.Api!");




app.Run();
