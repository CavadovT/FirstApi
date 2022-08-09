using FirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().HasData
                (
                new Product { Id = 1, Name = "Product1", Price = 25.5, IsActive = true },
                new Product { Id = 2, Name = "Product2", Price = 30.5, IsActive = true },
                new Product { Id = 3, Name = "Product3", Price = 27, IsActive = false },
                new Product { Id = 4, Name = "Product5", Price = 20, IsActive = true }
                );

        }

    }
}
