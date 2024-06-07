namespace ShoppingCart.Domain.Entities;

public record Cart
{
    public ICollection<CartItem> Items { get; set; } = [];
    public void Add(CartItem item)
    {
        Items.Add(item);
    }
}
