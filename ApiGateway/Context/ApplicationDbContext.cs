using ApiGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Context
{
    public class ApplicationDbContext  : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
