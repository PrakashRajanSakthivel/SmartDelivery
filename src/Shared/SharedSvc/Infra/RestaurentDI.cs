using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantService.Domain.Interfaces;
using RestaurantService.Infra.Data;
using RestaurantService.Infra.Repository;
using RestaurentService.Application.Restaurents.Handlers;
using RestaurentService.Application.Restaurents.Validators;
using RestaurentService.Domain.Interfaces;
using RestaurentService.Infra.Data;
using RestaurentService.Infra.Repository;
using SharedSvc.Common;
using SharedSvc.Validation;

namespace SharedSvc.Infra.Restaurant
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestaurentServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register RestaurantDbContext
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RestaurantDatabase")));

            services.AddScoped<IRestaurantUnitOfWork, RestaurantUnitOfWork>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            // Register validators
            services.AddValidatorsFromAssembly<CreateRestaurantValidator>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateRestaurantHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            // Add shared validation behavior
            services.AddValidationBehavior();

            return services;
        }
    }
}