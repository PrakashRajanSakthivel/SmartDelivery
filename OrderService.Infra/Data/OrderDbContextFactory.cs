
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OrderService.Infra.Data
{
    public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            // Get config from appsettings.json or environment
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Important: this resolves correctly when running CLI
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
            var connectionString = configuration.GetConnectionString("OrderDbConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new OrderDbContext(optionsBuilder.Options);
        }
    }
}


