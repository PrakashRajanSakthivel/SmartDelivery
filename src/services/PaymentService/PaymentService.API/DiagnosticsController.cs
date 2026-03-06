using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.CorrelationId;

namespace PaymentService.API
{
    [ApiController]
    [Route("api/diagnostics")]
    [AllowAnonymous]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(ILogger<DiagnosticsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Diagnostics ping on PaymentService. CorrelationId: {CorrelationId}", CorrelationContext.CorrelationId);
            return Ok(new
            {
                service = "PaymentService",
                host = Environment.MachineName,
                timestamp = DateTime.UtcNow,
                correlationId = CorrelationContext.CorrelationId
            });
        }
    }
}
