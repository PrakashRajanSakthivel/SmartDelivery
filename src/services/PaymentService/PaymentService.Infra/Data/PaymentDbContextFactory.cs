using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PaymentService.Infra.Data
{
    public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<PaymentDbContext>();
            var connectionString = configuration.GetConnectionString("PaymentDatabase") 
                ?? "Server=(localdb)\\mssqllocaldb;Database=PaymentServiceDb;Trusted_Connection=true;MultipleActiveResultSets=true";

            builder.UseSqlServer(connectionString);

            return new PaymentDbContext(builder.Options);
        }
    }
}