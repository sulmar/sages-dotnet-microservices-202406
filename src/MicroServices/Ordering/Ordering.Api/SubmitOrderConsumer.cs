using MassTransit;
using Shared.Models;

namespace Ordering.Api;

public class SubmitOrderConsumer : IConsumer<SubmitOrder>
{
    public Task Consume(ConsumeContext<SubmitOrder> context)
    {
        var order = context.Message.order;

        Console.WriteLine($"[Ordering.Api] Order received:");

        return Task.CompletedTask;
    }
}
