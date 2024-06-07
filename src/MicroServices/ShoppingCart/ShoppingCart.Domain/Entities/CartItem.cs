namespace ShoppingCart.Domain.Entities;

public record CartItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
