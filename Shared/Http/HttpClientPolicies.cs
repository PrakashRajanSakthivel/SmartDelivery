using OrderService.Application.Common;
using shared.CorrelationId;
using Polly.Extensions.Http;
using Polly;

namespace shared.Http
{
    // shared/Http/HttpClientPolicies.cs
    public static class HttpClientPolicies
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<CorrelationIdDelegatingHandler>();

            services.AddHttpClient<IRestaurentService, RestaurentService>(client =>
            {
                client.BaseAddress = new Uri("https://paymentservice.local");
            })
            .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

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
