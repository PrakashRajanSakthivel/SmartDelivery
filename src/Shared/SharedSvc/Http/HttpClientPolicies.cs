using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common;
using Polly;
using Polly.Extensions.Http;
using Shared.CorrelationId;
using PaymentService.Application.Services;

namespace Shared.Http
{
    // shared/Http/HttpClientPolicies.cs
    public static class HttpClientPolicies
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<CorrelationIdDelegatingHandler>();

            // Restaurant Service HTTP Client
            var restaurantServiceUrl = config["Services:RestaurantService:BaseUrl"] ?? "https://localhost:7001";
            services.AddHttpClient<IRestaurentService, OrderService.Application.Common.RestaurentService>(client =>
            {
                client.BaseAddress = new Uri(restaurantServiceUrl);
            })
            .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Simple Payment Service (no HTTP client needed)
            services.AddScoped<PaymentService.Application.Services.IExternalPaymentService, PaymentService.Application.Services.SimplePaymentService>();

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration config) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    config.GetValue<int>("ExternalPaymentProvider:RetryCount", 3),
                    retry => TimeSpan.FromSeconds(Math.Pow(2, retry)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} for payment service after {timespan} seconds");
                    });

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IConfiguration config) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    config.GetValue<int>("ExternalPaymentProvider:CircuitBreakerFailureThreshold", 5),
                    TimeSpan.FromSeconds(config.GetValue<int>("ExternalPaymentProvider:CircuitBreakerTimeout", 60)),
                    onBreak: (result, duration) => Console.WriteLine($"Payment service circuit breaker opened for {duration}"),
                    onReset: () => Console.WriteLine("Payment service circuit breaker closed"),
                    onHalfOpen: () => Console.WriteLine("Payment service circuit breaker half-open"));
    }
}
