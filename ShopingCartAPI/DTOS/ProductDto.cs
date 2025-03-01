using System.Text.Json.Serialization;

namespace ShopingCartAPI.DTOS
{
    public sealed record ProductDto
     (
         [property: JsonPropertyName("id")] Guid Id,
         [property: JsonPropertyName("name")] string Name,
         [property: JsonPropertyName("price")] decimal Price,
         [property: JsonPropertyName("stock")] int Stock
     );
}
