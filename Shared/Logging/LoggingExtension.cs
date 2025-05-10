using Shared.CorrelationId;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.Logging
{
    // shared/Logging/LoggingExtensions.cs
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseDefaultLogging(this IApplicationBuilder app, IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.With<CorrelationIdEnricher>()
                .CreateLogger();

            app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
            {
                Log.Information("App started.");
            });

            return app;
        }
    }

}
