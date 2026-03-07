using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService.Domain.Interfaces;
using RestaurantService.Infra.Data;
using RestaurentService.Application.Restaurents.Handlers;
using RestaurentService.Domain.Interfaces;
using RestaurentService.Infra.Data;
using RestaurentService.Infra.Repository;

namespace RestaurantService.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurentServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RestaurantDatabase")));

            services.AddScoped<IRestaurantUnitOfWork, RestaurantUnitOfWork>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateRestaurantHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            return services;
        }
    }
}
