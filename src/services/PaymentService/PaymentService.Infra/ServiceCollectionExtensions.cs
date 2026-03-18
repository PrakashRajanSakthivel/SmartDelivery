using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.common;
using PaymentService.Application.Payment.CommandHandlers;
using PaymentService.Domain;

namespace PaymentService.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentServiceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PaymentDatabase")));

            services.AddScoped<IPaymentIntentRepository, EfPaymentIntentRepository>();
            services.AddScoped<IPaymentUnitOfWork, PaymentUnitOfWork>();
            services.AddScoped<IPaymentService, DbPaymentService>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreatePaymentIntentCommandHandler).Assembly));

            return services;
        }
    }
}
