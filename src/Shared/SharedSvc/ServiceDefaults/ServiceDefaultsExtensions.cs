using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Authentication;
using Shared.CorrelationId;
using Shared.DevTools;
using Shared.Http;
using Shared.Logging;
using Shared.Swagger;
using SharedSvc.Exception;
using System.Threading.RateLimiting;

namespace Shared.ServiceDefaults
{
    /// <summary>
    /// Controls which optional cross-cutting concerns are applied.
    /// Use the defaults for most services; override for special cases (e.g. AuthService).
    /// </summary>
    public class ServiceDefaultsOptions
    {
        /// <summary>Register and use JWT Bearer authentication + authorization. Default: true.</summary>
        public bool UseJwt { get; set; } = true;

        /// <summary>Register and use the "AllowFrontend" CORS policy. Default: true.</summary>
        public bool UseCors { get; set; } = true;

        /// <summary>Map the /dev/token endpoint. Default: true, but it is only ever mapped
        /// when the host environment is Development.</summary>
        public bool MapDevToken { get; set; } = true;

        /// <summary>Redirect bare "/" requests to /swagger/index.html. Default: true.</summary>
        public bool RedirectRootToSwagger { get; set; } = true;
    }

    public static class ServiceDefaultsExtensions
    {
        /// <summary>
        /// Registers all common cross-cutting services (Serilog, controllers, Swagger,
        /// Correlation ID, optionally JWT and CORS).
        /// Call once at the top of Program.cs before builder.Build().
        /// </summary>
        public static WebApplicationBuilder AddServiceDefaults(
            this WebApplicationBuilder builder,
            string serviceName,
            ServiceDefaultsOptions? options = null)
        {
            options ??= new ServiceDefaultsOptions();

            builder.AddSerilogLogging(serviceName);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<CorrelationIdDelegatingHandler>();
            builder.Services.AddTransient<IstioTracingHeadersPropagationHandler>();
            builder.Services.AddControllers();
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerSupport()
                .AddCorrelationIdSupport();

            if (options.UseJwt)
                builder.Services.AddJwtAuth(builder.Configuration);

            if (options.UseCors)
            {
                builder.Services.AddCors(corsOptions =>
                {
                    corsOptions.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });
            }

            builder.Services.AddRateLimiter(rateLimitOptions =>
            {
                rateLimitOptions.GlobalLimiter = PartitionedRateLimiter.Create<Microsoft.AspNetCore.Http.HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 5
                        }));
                rateLimitOptions.RejectionStatusCode = 429;
            });

            return builder;
        }

        /// <summary>
        /// Wires the common middleware pipeline: Correlation ID, structured logging,
        /// optionally JWT + CORS, Swagger UI, dev token endpoint, root redirect,
        /// HTTPS redirection, and controller mapping.
        /// Call once after builder.Build().
        /// </summary>
        public static WebApplication UseServiceDefaults(
            this WebApplication app,
            IConfiguration configuration,
            ServiceDefaultsOptions? options = null)
        {
            options ??= new ServiceDefaultsOptions();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCorrelationId();
            app.UseDefaultLogging(configuration);
            app.UseRateLimiter();

            if (options.UseJwt)
                app.UseJwtAuth();
            else
                app.UseAuthorization();

            if (options.UseCors)
                app.UseCors("AllowFrontend");

            app.UseSwagger();
            app.UseSwaggerUI();

            // /dev/token mints a signed JWT for an anonymous caller, so it is gated on the
            // environment as well as the option — a service that forgets to opt out cannot
            // expose it in Production.
            if (options.MapDevToken && app.Environment.IsDevelopment())
                app.MapDevTokenGenerator(configuration);

            if (options.RedirectRootToSwagger)
            {
                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == "/")
                    {
                        context.Response.Redirect("/swagger/index.html");
                        return;
                    }
                    await next();
                });
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            return app;
        }
    }
}
