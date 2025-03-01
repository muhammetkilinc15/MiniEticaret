using Microsoft.EntityFrameworkCore;
using ShopingCartAPI.Models;

namespace ShopingCartAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
      public DbSet<ShopingCart> ShopingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopingCart>(opt =>
            {
                opt.Property(p=>p.Quantity).HasDefaultValue(0);
                opt.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
