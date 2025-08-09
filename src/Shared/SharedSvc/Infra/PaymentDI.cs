using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.common;
using PaymentService.Application.Payment.CommandHandlers;
using SharedSvc.Validation;

namespace SharedSvc.Infra.Payment
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Payment Service as Singleton to maintain payment intents across requests
            services.AddSingleton<IPaymentService, MockPaymentService>();

            // Register MediatR handlers
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreatePaymentIntentCommandHandler).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            // Add validation behavior
            services.AddValidationBehavior();

            return services;
        }
    }
} 