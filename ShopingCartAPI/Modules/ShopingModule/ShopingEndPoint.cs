using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopingCartAPI.Common.Results;
using ShopingCartAPI.Context;
using ShopingCartAPI.DTOS;
using ShopingCartAPI.Models;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ShopingCartAPI.Modules.ShopingModule
{
    public class ShopingEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            IEndpointRouteBuilder group = app.MapGroup("carts");
            group.MapGet("", async ([FromServices] IHttpClientFactory httpClientFactory, [FromServices] ApplicationDbContext context, CancellationToken cancellationToken) =>
            {
                var shopingCarts = await context.ShopingCarts.AsNoTracking().ToListAsync(cancellationToken);
                HttpClient client = httpClientFactory.CreateClient();
                // http://products-:8080/products
                HttpResponseMessage response = await client.GetAsync("http://products:8080/products", cancellationToken);
                Result<List<ProductDto>> products = new(new List<ProductDto>(), false, "Error");
                
                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);
                }
                else
                {
                    return Results.Ok(Result<List<ShopingCartDto>>.Fail("Products api ye bağlanamadı"));
                }
                List<ShopingCartDto> cartDtos = shopingCarts.Select(x => new ShopingCartDto
                (
                    x.Id,
                    x.ProductId,
                    products.Data.FirstOrDefault(p => p.Id == x.ProductId).Name,
                    x.Quantity,
                    products.Data.FirstOrDefault(p => p.Id == x.ProductId).Price
                )).ToList();
                return Results.Ok(Result<List<ShopingCartDto>>.Success(cartDtos));

            });

            group.MapPost("", async ([FromBody] CreateShopingCartDto request, [FromServices] ApplicationDbContext context, CancellationToken cancellationToken) =>
            {
                ShopingCart cart = new ShopingCart
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await context.AddAsync(cart, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                return Results.Ok(Result<string>.Success("Shoping Cart Created"));
            });
        }
    }
}
