using Carter;
using Microsoft.EntityFrameworkCore;
using MiniEticaret.Products.WebAPI.Context;
using MiniEticaret.Products.WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddCarter();

var app = builder.Build();

app.MapCarter();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.Run();
