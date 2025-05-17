using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shared.CorrelationId
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add(CorrelationIdHeader, correlationId);
            }

            CorrelationContext.CorrelationId = correlationId;
            context.Response.Headers.Add(CorrelationIdHeader, correlationId);

            await _next(context);
        }
    }
}
