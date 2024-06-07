namespace Shared.Models
{

    public record OrderDetail
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
