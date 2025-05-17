
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrderService.Infra.Data;
using OrderService.Infra.Repository;
using OrderService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common;
using OrderService.Application.Orders.Handlers;
using MediatR;



namespace Shared.Infra
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register OrderDbContext with the appropriate configuration
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderDatabase")));

            services.AddScoped<IRestaurentService, OrderService.Application.Common.RestaurentService>();
            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(UpdateOrderStatusCommandHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            // Add more services/repositories here later

            return services;
        }
    }
}


