
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CartService.Infra
{
    public class CartDbContextFactory : IDesignTimeDbContextFactory<CartDbContext>
    {
        public CartDbContext CreateDbContext(string[] args)
        {
            // Get config from appsettings.json or environment
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Important: this resolves correctly when running CLI
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CartDbContext>();
            var connectionString = configuration.GetConnectionString("CartDatabase");

            optionsBuilder.UseSqlServer(connectionString);

            return new CartDbContext(optionsBuilder.Options);
        }
    }
}


