using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopingCartAPI.Common.Results;
using ShopingCartAPI.Context;
using ShopingCartAPI.DTOS;
using ShopingCartAPI.Models;
using System.Linq;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ShopingCartAPI.Modules.ShopingModule
{
    public class ShopingEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            IEndpointRouteBuilder group = app.MapGroup("carts");
            group.MapGet("", async (IConfiguration configuration, [FromServices] IHttpClientFactory httpClientFactory, [FromServices] ApplicationDbContext context, CancellationToken cancellationToken) =>
            {
                List<ShopingCart> shopingCarts = await context.ShopingCarts.AsNoTracking().ToListAsync(cancellationToken);
                HttpClient client = httpClientFactory.CreateClient();
                // http://products-:8080/products

                var baseUrl = configuration.GetSection("HttpRequests:products").Value;
                HttpResponseMessage response = await client.GetAsync(baseUrl, cancellationToken);
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


            group.MapGet("/createOrder", async ([FromServices] IHttpClientFactory httpClientFactory, ApplicationDbContext context, IConfiguration configuration, CancellationToken cancellationToken) =>
            {
                List<ShopingCart> shopingCarts = await context.ShopingCarts.AsNoTracking().ToListAsync(cancellationToken);
                HttpClient client = httpClientFactory.CreateClient();
                var productBaseUrl = configuration.GetSection("HttpRequests:products").Value;
                HttpResponseMessage productResponse = await client.GetAsync(productBaseUrl, cancellationToken);
                Result<List<ProductDto>> products = new(new List<ProductDto>(), false, "Error");

                if (productResponse.IsSuccessStatusCode)
                {
                    products = await productResponse.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);
                }
                else
                {
                    return Results.Ok(Result<List<ShopingCartDto>>.Fail("Products api ye bağlanamadı"));
                }
                List<CreateOrderDto> response = shopingCarts.Select(x => new CreateOrderDto
                (
                   x.ProductId,
                   x.Quantity,
                   products!.Data!.Find(p => p.Id == x.ProductId).Price
                )).ToList();
                var ordersEndpoint = configuration.GetSection("HttpRequests:orders").Value;
                HttpResponseMessage orderMessage = await client.PostAsJsonAsync(ordersEndpoint, response, cancellationToken);
                if (orderMessage.IsSuccessStatusCode)
                {
                    List<ChangeProductStockDto> change = shopingCarts.Select(x => new ChangeProductStockDto
                    (
                        x.ProductId,
                        x.Quantity
                    )).ToList();
                    HttpResponseMessage stockResponse = await client.PutAsJsonAsync($"{productBaseUrl}/change-product-stock", change, cancellationToken);
                    context.ShopingCarts.RemoveRange(shopingCarts);
                    await context.SaveChangesAsync(cancellationToken);
                    return Results.Ok(Result<string>.Success("Order Created"));
                }
                else
                {
                    return Results.Ok(Result<string>.Fail("Order apiye bağlanamadı"));
                }
            }).Produces<string>();

        }
    }
}
