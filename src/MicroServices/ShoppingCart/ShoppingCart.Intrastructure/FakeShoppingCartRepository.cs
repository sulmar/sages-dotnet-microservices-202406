using ShoppingCart.Domain.Abstractions;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Intrastructure;

public class FakeShoppingCartRepository(Context _context) : IShoppingCartRepository
{
    public void Add(Product product)
    {
        if (_context.Products.Contains(product))
        {
            _context.Products.TryGetValue(product, out var _product);
            _product.Quantity++;
        }
        else
            _context.Products.Add(product);
    }

    public void Clear() => _context.Products.Clear();
}


public class Context
{
    public HashSet<Product> Products { get; set; } = new HashSet<Product>();
}