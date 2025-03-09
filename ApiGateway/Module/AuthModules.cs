using ApiGateway.Common.Results;
using ApiGateway.Context;
using ApiGateway.Dtos.AuthDtos;
using ApiGateway.Models;
using ApiGateway.Module.Abstractions;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Module
{
    public class AuthModules : IModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            IEndpointRouteBuilder group = app.MapGroup("/auth");

            group.MapPost("/login", async (ApplicationDbContext context, [FromBody] LoginDto request, [FromServices] IJwtProvider jwtProvider, CancellationToken cancellationToken) =>
            {
                User? user = await context.Users.FirstOrDefaultAsync(x => x.UserName == x.UserName);
                if (user is null)
                {
                    Results.BadRequest(Result<string>.Fail("User not found"));
                }
                // Token generation logic
                string token = jwtProvider.CreateToken(user);
                return Results.Ok(Result<string>.Success(token));
            });

            group.MapPost("/register", async (ApplicationDbContext context, [FromBody] RegisterDto request, CancellationToken cancellationToken) =>
            {
                bool user = await context.Users.AnyAsync(x => x.UserName == x.UserName);
                if (user)
                {
                    Results.BadRequest(Result<string>.Fail("User already exists"));
                }

                User newUser = new User
                {
                    UserName = request.UserName,
                    Password = request.Password
                };

                await context.Users.AddAsync(newUser, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(Result<string>.Success("User created successfully"));
            });
        }
    }
}
