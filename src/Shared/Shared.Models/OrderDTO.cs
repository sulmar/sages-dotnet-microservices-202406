namespace Shared.Models
{
    public record OrderDTO
    {
        public IEnumerable<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }

}
