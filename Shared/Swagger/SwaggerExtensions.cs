using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace shared.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerSupport(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DeliveryApp API",
                    Version = "v1"
                });
            });
            return services;
        }
    }
}
