
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

            // Register HTTP client for restaurant service communication
            services.AddHttpClient<IRestaurentService, OrderService.Application.Common.RestaurentService>(client =>
            {
                client.BaseAddress = new Uri(configuration["RestaurantService:BaseUrl"] ?? "http://localhost:5001");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Register business validation service
            services.AddScoped<IOrderBusinessValidationService, OrderBusinessValidationService>();

            services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddValidatorsFromAssembly<CreateOrderRequestValidator>();
            services.AddValidatorsFromAssembly<UpdateOrderStatusCommandValidator>();
            
            // Register MediatR from Application assembly (like Restaurant service)
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddValidationBehavior();

            return services;
        }
    }
}


