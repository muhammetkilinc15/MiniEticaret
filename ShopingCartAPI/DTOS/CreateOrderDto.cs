namespace ShopingCartAPI.DTOS
{
    public sealed record CreateOrderDto
    (
        Guid ProductId,
        int Quantity,
        decimal Price
    );
}
