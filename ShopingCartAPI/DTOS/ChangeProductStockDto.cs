namespace ShopingCartAPI.DTOS
{
    public sealed record ChangeProductStockDto
     (
         Guid ProductId,
         int Quantity
     );
}
