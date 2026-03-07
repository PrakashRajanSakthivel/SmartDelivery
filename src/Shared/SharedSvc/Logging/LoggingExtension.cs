using Shared.CorrelationId;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shared.Logging
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// Configures the bootstrap Serilog logger (Console + Elasticsearch) and registers
        /// it with the host. Call this once at the top of Program.cs before building services.
        /// The index format is derived automatically as "{serviceName}-logs-{yyyy.MM.dd}".
        /// </summary>
        public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder, string serviceName)
        {
            builder.Logging.ClearProviders();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var elasticUri = configuration["Elasticsearch:Uri"];

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri ?? "http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{serviceName.ToLower()}-logs-{{0:yyyy.MM.dd}}",
                    CustomFormatter = new ElasticsearchJsonFormatter(renderMessage: true),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.RaiseCallback
                })
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }

        /// <summary>
        /// Replaces the logger with a fully config-driven instance (reads from appsettings Serilog
        /// section), adds per-request structured logging, and fires a startup log on ApplicationStarted.
        /// Call this in the middleware pipeline after CorrelationIdMiddleware.
        /// </summary>
        public static IApplicationBuilder UseDefaultLogging(this IApplicationBuilder app, IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.With<CorrelationIdEnricher>()
                .CreateLogger();

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
                options.GetLevel = (ctx, elapsed, ex) =>
                    ex != null ? LogEventLevel.Error :
                    ctx.Response.StatusCode > 499 ? LogEventLevel.Error :
                    LogEventLevel.Information;
            });

            app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
            {
                Log.Information("App started.");
            });

            return app;
        }
    }
}
