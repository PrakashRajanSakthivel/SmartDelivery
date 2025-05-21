using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands;
using MediatR;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Queries;
using OrderService.Application.Orders.QueriesHandlers;
using OrderService.Application.Orders.Queries;

namespace OrderService.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(request);
            var orderId = await _mediator.Send(command);
            return CreatedAtAction(
                   nameof(GetOrderById),
                   new { id = orderId },
                   new
                   {  // Response body
                       id = orderId,
                       _links = new
                       {
                           self = Url.Link(nameof(GetOrderById), new { id = orderId })
                       }
                   });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            _logger.LogInformation("Fetching order with ID: {OrderId}", id);
            _logger.LogDebug("Fetching order with ID: {OrderId}", id);
            _logger.LogWarning("Fetching order with ID: {OrderId}", id);
            var query = new GetOrderById(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            if (orderId != command.OrderId)
                return BadRequest("OrderId mismatch");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("logging-test")]
        public IActionResult LoggingTest()
        {
            _logger.LogInformation("Test information message");
            _logger.LogWarning("Test warning message");
            _logger.LogError("Test error message");

            try
            {
                throw new Exception("This is a test exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test exception with stack trace");
            }

            return Ok(new
            {
                Message = "Test logs generated",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


