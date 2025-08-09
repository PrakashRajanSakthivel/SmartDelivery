using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common;
using Polly;
using Polly.Extensions.Http;
using Shared.CorrelationId;
using PaymentService.Application.common;

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

            // var paymentServiceUrl = config["Services:PaymentService:BaseUrl"] ?? "https://localhost:7003";
            // services.AddHttpClient<IPaymentService, PaymentServiceHttpClient>(client =>
            // {
            //     client.BaseAddress = new Uri(paymentServiceUrl);
            // })
            // .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
            // .AddPolicyHandler(GetRetryPolicy())
            // .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
