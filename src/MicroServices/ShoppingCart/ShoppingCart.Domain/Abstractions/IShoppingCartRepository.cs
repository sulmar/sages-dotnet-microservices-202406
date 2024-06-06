using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Domain.Abstractions;

public interface IShoppingCartRepository
{
    void Add(Product product);
    void Clear();
}
