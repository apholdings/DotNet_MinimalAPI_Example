using DotNet_MinimalAPI_Example.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet_MinimalAPI_Example.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    ProductId = 1,
                    Name = "Antena Wifi",
                    Price = 69.00,
                    Description = "Tis a product description",
                    CategoryName = "Antenas",
                    ImageUrl = "https://boomslag.s3.us-east-2.amazonaws.com/lightbulb.jpg"
                },
                new Product()
                {
                    ProductId = 2,
                    Name = "Antonella",
                    Price = 1169.00,
                    Description = "Antos ass is delicious",
                    CategoryName = "Antenas",
                    ImageUrl = "https://boomslag.s3.us-east-2.amazonaws.com/lightbulb.jpg"
                }
                );
        }
    }
}
