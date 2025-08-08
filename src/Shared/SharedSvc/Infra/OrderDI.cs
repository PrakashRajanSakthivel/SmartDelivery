
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common;
using OrderService.Application.Orders.Handlers;
using OrderService.Application.Orders.Validators;
using OrderService.Domain.Interfaces;
using OrderService.Infra.Data;
using OrderService.Infra.Repository;
using SharedSvc.Validation;



namespace SharedSvc.Infra.Order
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

            services.AddValidatorsFromAssembly<CreateOrderCommandValidator>();

            services.AddValidatorsFromAssembly<UpdateOrderStatusCommandValidator>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(UpdateOrderStatusHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddValidationBehavior();

            return services;
        }
    }
}


