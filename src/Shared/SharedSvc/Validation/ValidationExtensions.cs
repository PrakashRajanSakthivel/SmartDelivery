using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SharedSvc.Validation
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidationBehavior(this IServiceCollection services)
        {
            // Register validation behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddValidatorsFromAssembly<T>(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<T>();

            return services;
        }
    }
}
