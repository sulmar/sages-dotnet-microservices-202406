using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging;

public static class Extensions
{   
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly? assembly = null)
    {
        services.AddMassTransit(x =>
        {
            var host = new Uri(configuration["MessageBroker:Host"]);
            var username = configuration["MessageBroker:UserName"];
            var password = configuration["MessageBroker:Password"];

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, host =>
                {
                    host.Username(username);
                    host.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });           
        });

        return services;
    }
}
