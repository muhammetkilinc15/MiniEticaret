namespace ShopingCartAPI.Models
{
    public sealed class ShopingCart
    {
        public Guid Id { get; set; }
        public ShopingCart()
        {
            Id = Guid.NewGuid();
        }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
