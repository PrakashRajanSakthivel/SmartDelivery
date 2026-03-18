using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PaymentService.Infra
{
    public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PaymentDatabase"));

            return new PaymentDbContext(optionsBuilder.Options);
        }
    }
}
