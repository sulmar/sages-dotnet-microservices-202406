namespace Catalog.Domain.Entities;

public abstract class EntityBase
{
    public int Id { get; set; }
}

public class Product : EntityBase
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}