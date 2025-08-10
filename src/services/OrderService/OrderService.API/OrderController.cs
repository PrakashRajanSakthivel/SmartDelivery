using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands;
using OrderService.Application.Orders.DTO;
using OrderService.Application.Orders.Queries;
using SharedSvc.Response;

namespace OrderService.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {
        public OrderController(IMediator mediator, ILogger<OrderController> logger)
           : base(mediator, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(request);
            var order = await _mediator.Send(command);

            return Created(order, "Order created successfully");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            _logger.LogInformation("Fetching order with ID: {OrderId}", id);
            _logger.LogDebug("Fetching order with ID: {OrderId}", id);
            _logger.LogWarning("Fetching order with ID: {OrderId}", id);
            _logger.LogInformation("Fetching order with ID: {OrderId}", id);

            var query = new GetOrderById(id);
            var result = await _mediator.Send(query);

            return Success(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] Guid? userId = null)
        {
            _logger.LogInformation("Fetching orders. UserId: {UserId}", userId);
            
            if (userId.HasValue)
            {
                var query = new GetOrdersByUserId(userId.Value);
                var result = await _mediator.Send(query);
                return Success(result);
            }
            else
            {
                var query = new GetAllOrders();
                var result = await _mediator.Send(query);
                return Success(result);
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] UpdateOrderRequest request)
        {
            if (orderId != request.OrderId)
                return BadRequest("OrderId mismatch");

            var command = new UpdateOrderCommand(request);
            var result = await _mediator.Send(command);

            return Success(result, "Order updated successfully");
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            if (orderId != request.OrderId)
                return BadRequest("OrderId mismatch");

            var command = new UpdateOrderStatusCommand(
                request.OrderId,
                request.NewStatus,
                request.Reason);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{orderId}/status")]
        public async Task<IActionResult> GetStatus(Guid orderId)
        {
            throw new NotImplementedException("Pending...");
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


