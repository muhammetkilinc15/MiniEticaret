namespace OrderAPI.Models
{
    public sealed class Order
    {
        public Guid Id { get; set; }
        public Order()
        {
            Id = Guid.NewGuid();
        }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
