namespace ShoppingCart.Api;

public record CreateOrderRequest;

public record CartDTO
{
    public IEnumerable<CartItemDTO> Items { get; set; }
}

public record CartItemDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
