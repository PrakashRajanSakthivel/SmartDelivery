using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SharedSvc.HealthChecks;

public static class CustomHealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, CustomHealthCheckOptions options)
    {
        var healthChecks = services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy("Service is healthy"));

        // Add database check if enabled and connection string provided
        if (options.EnableDatabaseCheck && !string.IsNullOrEmpty(options.DatabaseConnectionString))
        {
            healthChecks.AddSqlServer(
                connectionString: options.DatabaseConnectionString,
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "db", "sql", "sqlserver", "ready" });
        }

        // Add Elasticsearch check if enabled and URI provided
        if (options.EnableElasticsearchCheck && !string.IsNullOrEmpty(options.ElasticsearchUri))
        {
            healthChecks.AddUrlGroup(
                uris: new Uri[] { new Uri(options.ElasticsearchUri) },
                name: "elasticsearch",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "external", "elasticsearch" });
        }

        return services;
    }

    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app, string serviceName)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = (context, report) => WriteHealthCheckResponse(context, report, serviceName),
            AllowCachingResponses = false
        });

        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready") || check.Name == "self",
            ResponseWriter = (context, report) => WriteHealthCheckResponse(context, report, serviceName),
            AllowCachingResponses = false
        });

        app.UseHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // Only self check for liveness
            ResponseWriter = (context, report) => WriteHealthCheckResponse(context, report, serviceName),
            AllowCachingResponses = false
        });

        app.UseHealthChecks("/ping", new HealthCheckOptions
        {
            Predicate = _ => false, // Only self check
            ResponseWriter = (context, report) => WriteHealthCheckResponse(context, report, serviceName),
            AllowCachingResponses = false
        });

        return app;
    }

    private static async Task WriteHealthCheckResponse(HttpContext context, HealthReport report, string serviceName)
    {
        context.Response.ContentType = "application/json";

        var startTime = DateTime.UtcNow.AddSeconds(-Environment.TickCount64 / 1000.0);
        var uptime = DateTime.UtcNow - startTime;

        var response = new
        {
            Status = report.Status.ToString(),
            Service = serviceName,
            Timestamp = DateTime.UtcNow,
            Uptime = uptime,
            Version = "1.0.0",
            Checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    Status = entry.Value.Status.ToString(),
                    Description = entry.Value.Description,
                    Duration = entry.Value.Duration,
                    Tags = entry.Value.Tags
                })
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
