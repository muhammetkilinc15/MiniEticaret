using Carter;
using OrderAPI.Context;
using OrderAPI.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();


builder.Services.AddHttpClient();

builder.Services.AddOptions<MongoDbSettings>().Bind(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbContext>();
var app = builder.Build();

app.MapCarter();

app.MapGet("/", () => "Hello World!");

app.Run();
