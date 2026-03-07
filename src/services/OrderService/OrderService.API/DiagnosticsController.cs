using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Common;
using Shared.CorrelationId;
using System.Text.Json;

namespace OrderService.API
{
    [ApiController]
    [Route("api/diagnostics")]
    [AllowAnonymous]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IRestaurentService _restaurentService;
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(IRestaurentService restaurentService, ILogger<DiagnosticsController> logger)
        {
            _restaurentService = restaurentService;
            _logger = logger;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Diagnostics ping on OrderService. CorrelationId: {CorrelationId}", CorrelationContext.CorrelationId);
            return Ok(new
            {
                service = "OrderService",
                host = Environment.MachineName,
                timestamp = DateTime.UtcNow,
                correlationId = CorrelationContext.CorrelationId
            });
        }

        [HttpGet("chain")]
        public async Task<IActionResult> Chain()
        {
            _logger.LogInformation("Diagnostics chain initiated from OrderService. CorrelationId: {CorrelationId}", CorrelationContext.CorrelationId);

            var rawResponse = await _restaurentService.PingAsync();
            var downstream = JsonSerializer.Deserialize<JsonElement>(rawResponse);

            _logger.LogInformation("Diagnostics chain completed — RestaurantService responded. CorrelationId: {CorrelationId}", CorrelationContext.CorrelationId);

            return Ok(new
            {
                service = "OrderService",
                host = Environment.MachineName,
                timestamp = DateTime.UtcNow,
                correlationId = CorrelationContext.CorrelationId,
                downstream
            });
        }
    }
}
