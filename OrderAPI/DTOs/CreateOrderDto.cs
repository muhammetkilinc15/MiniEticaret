namespace OrderAPI.DTOs
{
    public sealed record CreateOrderDto
   (
    Guid ProductId, int Quantity, decimal Price);

    public sealed record OrderDto
    (
        Guid Id, 
        Guid ProductId,
        string ProductName,
        int Quantity, 
        decimal Price, 
        DateTime CreatedAt
     );

}
