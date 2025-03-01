namespace MiniEticaret.Products.WebAPI.DTOs
{
    public sealed record CreatProductDto
   (
        string Name,
        decimal Price,
        int Stock
   );
}
