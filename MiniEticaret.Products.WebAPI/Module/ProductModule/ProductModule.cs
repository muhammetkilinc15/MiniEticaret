using Carter;
using Microsoft.EntityFrameworkCore;
using MiniEticaret.Products.WebAPI.Common.Results;
using MiniEticaret.Products.WebAPI.Context;
using MiniEticaret.Products.WebAPI.DTOs;
using MiniEticaret.Products.WebAPI.Models;

namespace MiniEticaret.Products.WebAPI.Module.ProductModule
{
    public class ProductModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            IEndpointRouteBuilder group = app.MapGroup("/products");


            group.MapGet("/seedData", async (ApplicationDbContext context) =>
            {

                Product[] products = new Product[]
                {
                    new Product{Name="Product 1",Price=100,Stock=100},
                    new Product{Name="Product 2",Price=200,Stock=200},
                    new Product{Name="Product 3",Price=300,Stock=300},
                    new Product{Name="Product 4",Price=400,Stock=400},
                    new Product{Name="Product 5",Price=500,Stock=500},
                };
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();

                return Results.Ok(Result<string>.Success("Seed data added"));


            }).Produces<Result<string>>();


            group.MapGet("", async (ApplicationDbContext context) =>
            {
                var products = await context.Products.OrderBy(x => x.Name).AsNoTracking().ToListAsync();
                return Results.Ok(Result<List<Product>>.Success(products));

            }).Produces<Result<List<Product>>>();

            group.MapGet("/{id:guid}", async (ApplicationDbContext context, Guid id) =>
            {
                var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product is null)
                {
                    return Results.NotFound(Result<Product>.Fail("Product not found"));
                }
                return Results.Ok(Result<Product>.Success(product));
            }).Produces<Result<Product>>();

            group.MapPost("", async (ApplicationDbContext context, CreatProductDto product, CancellationToken cancellationToken) =>
            {

                bool isNameExist = await context.Products.AnyAsync(x => x.Name == product.Name, cancellationToken);
                if (isNameExist)
                {
                    return Results.BadRequest(Result<Product>.Fail("Product name already exists"));
                }

                Product p = new()
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock
                };
                context.Products.Add(p);
                await context.SaveChangesAsync(cancellationToken);
                return Results.Ok(Result<Product>.Success(p));
            }).Produces<Result<Product>>();

            group.MapPut("/{id:guid}", async (ApplicationDbContext context, Guid id, CreatProductDto product, CancellationToken cancellationToken) =>
            {
                var p = await context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (p is null)
                {
                    return Results.NotFound(Result<Product>.Fail("Product not found"));
                }
                p.Name = product.Name;
                p.Price = product.Price;
                p.Stock = product.Stock;
                await context.SaveChangesAsync(cancellationToken);
                return Results.Ok(Result<Product>.Success(p));
            }).Produces<Result<Product>>();

            group.MapDelete("/{id:guid}", async (ApplicationDbContext context, Guid id, CancellationToken cancellationToken) =>
            {
                var p = await context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (p is null)
                {
                    return Results.NotFound(Result<Product>.Fail("Product not found"));
                }
                context.Products.Remove(p);
                await context.SaveChangesAsync(cancellationToken);
                return Results.Ok(Result<Product>.Success(p));
            }).Produces<Result<Product>>();

            group.MapPut("/change-product-stock", async (List<ChangeProductStockDto> request, ApplicationDbContext context, CancellationToken cancellationToken) =>
            {
                foreach (var item in request)
                {
                    Product product = await context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId, cancellationToken);
                    if (product is not null)
                    {
                        if (product.Stock > 0)
                            product.Stock -= item.Quantity;
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }
                return Results.Created();

            });

        }
    }
}
