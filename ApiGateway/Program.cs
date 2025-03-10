using ApiGateway.Context;
using ApiGateway.Module.Abstractions;
using ApiGateway.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ⬇️ Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgress"));
});

builder.Services.AddScoped<IJwtProvider, JwtProvider>(); // ✅ Buraya alındı!

// ⬇️ JWT ayarları
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// ⬇️ CORS ayarları
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();

    });
});

// Rate Limiting

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(15); // 15 saniyede bir sıfırlanacak
        options.PermitLimit = 5; // 15 saniyede maksimum 5 istek alınacak
        options.QueueLimit = 5; // 15 saniyede 5'ten fazla istek gelirse kuyruğa alınacak
    });
});


// ⬇️ Reverse Proxy ayarları
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ⬇️ Authentication ve Authorization servislerini `builder.Build()` ÖNCESİNDE eklemelisin!
builder.Services.AddAuthentication().AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? "")
        ),
    };
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Authenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
}); // ✅ Buraya alındı!

var app = builder.Build(); // 🔥 Artık servis koleksiyonuna ekleme yapılamaz!

// ⬇️ Middleware ve Uygulama Konfigürasyonu

app.UseAuthentication(); // ✅ Authentication middleware'i eklenmeli
app.UseAuthorization(); // ✅ Authorization middleware'i eklenmeli
app.UseCors();

app.UseRateLimiter();
app.MapReverseProxy();

// ⬇️ Modülleri yükleme
var modules = typeof(Program).Assembly.GetTypes()
    .Where(t => t.IsAssignableTo(typeof(IModule)) && !t.IsAbstract)
    .Select(Activator.CreateInstance)
    .Cast<IModule>();

foreach (var module in modules)
{
    module.AddRoutes(app);
}

// ⬇️ Veritabanı migrasyonlarını çalıştırma
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
