namespace ShopingCartAPI.DTOS
{
    public sealed record CreateShopingCartDto
    (
         Guid ProductId,
         int Quantity
    );

    public sealed record ShopingCartDto
    (
            Guid Id,
            Guid ProductId,
            string ProductName,
            int Quantity,
            decimal Price
    );

}
