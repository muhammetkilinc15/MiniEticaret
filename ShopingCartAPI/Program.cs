using Carter;
using Microsoft.EntityFrameworkCore;
using ShopingCartAPI.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.AddCarter();

var app = builder.Build();

app.MapCarter();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
     dbContext.Database.Migrate();
}

app.Run();
