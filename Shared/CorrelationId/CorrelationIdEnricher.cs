using Serilog.Core;
using Serilog.Events;

namespace Shared.CorrelationId
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!string.IsNullOrEmpty(CorrelationContext.CorrelationId))
            {
                var correlationIdProperty = propertyFactory.CreateProperty(
                    "CorrelationId", CorrelationContext.CorrelationId);
                logEvent.AddOrUpdateProperty(correlationIdProperty);
            }
        }
    }
}
