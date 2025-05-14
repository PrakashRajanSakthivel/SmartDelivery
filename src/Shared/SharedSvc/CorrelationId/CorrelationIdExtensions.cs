using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.CorrelationId
{
    public static class CorrelationIdExtensions
    {
        public static IServiceCollection AddCorrelationIdSupport(this IServiceCollection services)
        {
            services.AddTransient<CorrelationIdMiddleware>();
            services.AddTransient<CorrelationIdDelegatingHandler>();
            return services;
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
