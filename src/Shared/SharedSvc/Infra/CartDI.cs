using CartService.Application.Handlers;
using CartService.Domain.Interfaces;
using CartService.Infra;
using CartService.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedSvc.Infra.Cart
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCartServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CartDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CartDatabase")));

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartUnitOfWork, CartUnitOfWork>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(AddCartItemHandler).Assembly));

            return services;
        }
    }
}
