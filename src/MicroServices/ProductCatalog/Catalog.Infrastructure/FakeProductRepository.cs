using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure;

public class FakeProductRepository(Context _context) : IProductRepository
{
    public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult<IEnumerable<Product>>(_context.Products.Values);
    public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_context.Products[id]);
}

public class Context
{
    public IDictionary<int, Product> Products = new Dictionary<int, Product>();
}