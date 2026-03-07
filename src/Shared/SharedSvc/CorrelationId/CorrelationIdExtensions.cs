using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.CorrelationId
{
    public static class CorrelationIdExtensions
    {
        public static IServiceCollection AddCorrelationIdSupport(this IServiceCollection services)
        {
            // CorrelationIdDelegatingHandler is registered in AddServiceDefaults.
            // CorrelationIdMiddleware must NOT be registered here — RequestDelegate is
            // injected by UseMiddleware<T>() at pipeline time, not from the DI container.
            return services;
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
