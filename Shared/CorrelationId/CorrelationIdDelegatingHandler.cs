namespace shared.CorrelationId
{
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(CorrelationIdHeader))
            {
                var correlationId = CorrelationContext.CorrelationId ?? Guid.NewGuid().ToString();
                request.Headers.Add(CorrelationIdHeader, correlationId);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
