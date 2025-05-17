using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Interfaces;
using OrderService.Infra.Data;
using RestaurantService.Domain.Interfaces;
using RestaurantService.Infra.Data;
using RestaurentService.Application.Restaurents.Handlers;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Application.Services;
using RestaurentService.Domain.Interfaces;
using RestaurentService.Infra.Data;
using RestaurentService.Infra.Repository;

namespace SharedSvc.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurentServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register OrderDbContext with the appropriate configuration
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RestaurantDatabase")));

            services.AddScoped<IRestaurantUnitOfWork, RestaurantUnitOfWork>();

            // Register services
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateRestaurantHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            // Add more services/repositories here later

            return services;
        }
    }
}
