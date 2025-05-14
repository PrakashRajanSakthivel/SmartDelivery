using OrderService.Application.Model;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Services;
using OrderService.Application.Orders.Commands;
using MediatR;

namespace OrderService.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMediator _mediator;

        public OrderController(IOrderService orderService, IMediator mediator)
        {
            _orderService = orderService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var orderId = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            // We'll implement this later.
            return Ok();
        }

        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            if (orderId != command.OrderId)
                return BadRequest("OrderId mismatch");

            await _mediator.Send(command);
            return NoContent();
        }
    }

}
