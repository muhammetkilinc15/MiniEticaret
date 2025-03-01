using Carter;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrderAPI.Commons.Result;
using OrderAPI.Context;
using OrderAPI.DTOs;
using OrderAPI.Models;

namespace OrderAPI.Module.OrderModule
{
    public sealed class OrderEnpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            IEndpointRouteBuilder group = app.MapGroup("orders");

            group.MapGet("", async (IHttpClientFactory httpClientFactory, IConfiguration configure, MongoDbContext context, CancellationToken cancellationToken) =>
            {
                var items = context.GetCollection<Order>("orders");

                Result<List<ProductDto>>? products = new();

                HttpClient client = httpClientFactory.CreateClient();

                string apiUrl = configure.GetSection("HttpProductAPI:BaseUrl").Value!;

                // http://products:8080/products
                HttpResponseMessage message = await client.GetAsync(apiUrl, cancellationToken);

                if (message.IsSuccessStatusCode)
                {
                    products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>(cancellationToken);

                }
                else
                {
                    return Results.BadRequest(Result<List<OrderDto>>.Failure("Failed to fetch products"));
                }

                var orders = await items.Find(item => true).ToListAsync(cancellationToken);

                List<OrderDto> orderDtos = new();
                foreach (var order in orders)
                {
                    orderDtos.Add(
                        new OrderDto(
                            order.Id,
                            order.ProductId,
                            products!.Data!.First(x => x.Id == order.ProductId).Name,
                            order.Quantity,
                            order.Price,
                            order.CreatedAt
                        )
                    );
                }
                return Results.Ok(Result<List<OrderDto>>.Success(orderDtos));
            });


            group.MapPost("", async ([FromBody] List<CreateOrderDto> request, [FromServices] MongoDbContext context) =>
            {
                List<Order> orders = new();
                var items = context.GetCollection<Order>("orders");
                foreach (var item in request)
                {
                    Order order = new()
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        CreatedAt = DateTime.Now
                    };
                    orders.Add(order);
                }
                await items.InsertManyAsync(orders);
                return Results.Ok(Result<string>.Success("Orders created successfully"));
            });
        }
    }
}
