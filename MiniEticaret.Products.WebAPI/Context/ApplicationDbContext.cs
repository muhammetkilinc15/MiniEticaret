using Microsoft.EntityFrameworkCore;
using MiniEticaret.Products.WebAPI.Models;

namespace MiniEticaret.Products.WebAPI.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x=>x.Price).HasColumnType("money");
            base.OnModelCreating(modelBuilder);
        }

        protected ApplicationDbContext()
        {
        }
    }
}
