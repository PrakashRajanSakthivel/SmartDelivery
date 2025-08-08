using CartService.Application.Handlers;
using CartService.Application.Validators;
using CartService.Domain.Interfaces;
using CartService.Infra;
using CartService.Infra.Data;
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

            services.AddScoped<ICartUnitOfWork, CartUnitOfWork>();

            services.AddValidatorsFromAssembly<AddCartItemRequestValidator>();
            services.AddValidatorsFromAssembly<CartService.Application.Validators.UpdateCartItemRequestValidator>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AddCartItemHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddValidationBehavior();

            services.AddAutoMapper(typeof(CartService.Application.Mapper.CartProfile));

            return services;
        }
    }
}
