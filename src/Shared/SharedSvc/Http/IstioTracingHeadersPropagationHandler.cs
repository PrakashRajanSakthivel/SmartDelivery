using Microsoft.AspNetCore.Http;

namespace Shared.Http
{
    /// <summary>
    /// Propagates Istio/Envoy distributed-tracing headers (B3 multi-header and W3C TraceContext)
    /// from the current inbound HTTP request to every outbound HttpClient call.
    /// Without this, Envoy sidecars cannot stitch a single distributed trace across services.
    /// </summary>
    public class IstioTracingHeadersPropagationHandler : DelegatingHandler
    {
        private static readonly string[] TracingHeaders =
        [
            "x-request-id",
            "traceparent",
            "tracestate",
            "x-b3-traceid",
            "x-b3-spanid",
            "x-b3-parentspanid",
            "x-b3-sampled",
            "x-b3-flags",
        ];

        private readonly IHttpContextAccessor _httpContextAccessor;

        public IstioTracingHeadersPropagationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var incomingHeaders = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (incomingHeaders is not null)
            {
                foreach (var header in TracingHeaders)
                {
                    if (incomingHeaders.TryGetValue(header, out var value)
                        && !request.Headers.Contains(header))
                    {
                        request.Headers.TryAddWithoutValidation(header, (string)value);
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
