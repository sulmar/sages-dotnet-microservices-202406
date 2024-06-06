using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;
using StackExchange.Redis;

namespace ShoppingCart.Intrastructure;

// dotnet add package StackExchange.Redis
public class RedisShoppingCartRepository(IConnectionMultiplexer _connectionMultiplexer) : IShoppingCartRepository
{
    public void Add(Product product)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var userId = "123";

        string cartKey = $"cart:{userId}";
        string field = $"product:{product.Id}";

        db.HashIncrement(cartKey, field, product.Quantity);
    }

    public void Clear()
    {
        var db = _connectionMultiplexer.GetDatabase();

        var userId = "123";

        string cartKey = $"cart:{userId}";

        db.KeyDelete(cartKey);
    }
}