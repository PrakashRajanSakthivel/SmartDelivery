using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Payments.Handlers;
using PaymentService.Domain.Interfaces;
using PaymentService.Infra.Data;
using PaymentService.Infra.Repository;
using SharedSvc.Validation;

namespace SharedSvc.Infra.Payment
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register PaymentDbContext with the appropriate configuration
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PaymentDatabase")));

            // Register repositories and unit of work
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentUnitOfWork, PaymentUnitOfWork>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(PaymentService.Application.Mapper.PaymentProfile));

            // Register MediatR handlers
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreatePaymentHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            // Add validation behavior
            services.AddValidationBehavior();

            return services;
        }
    }
} 