using OrderService.Domain.Interfaces;
using OrderService.Infra.Data;
using OrderService.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace shared.Infra
{
   
public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register OrderDbContext with the appropriate configuration
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderDbConnection")));

            services.AddScoped<IOrderRepository, OrderRepository>();
            // Add more services/repositories here later

            return services;
        }
    }
    }


