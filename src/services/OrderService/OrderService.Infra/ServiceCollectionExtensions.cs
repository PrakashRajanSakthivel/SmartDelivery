using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Interfaces;
using OrderService.Infra.Data;
using OrderService.Infra.Repository;

namespace OrderService.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderDatabase")));

            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
