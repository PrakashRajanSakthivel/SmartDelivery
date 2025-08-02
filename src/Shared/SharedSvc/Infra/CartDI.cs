using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CartService.Domain.Interfaces;
using CartService.Infra;

namespace SharedSvc.Infra.Cart
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCartServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register CartDbContext with the appropriate configuration
            services.AddDbContext<CartDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CartDatabase")));

            services.AddScoped<ICartRepository, CartRepository>();

            // Add more services/repositories here later

            return services;
        }
    }
}
