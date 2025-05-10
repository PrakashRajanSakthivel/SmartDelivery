using OrderService.Application.Model;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Services;

namespace OrderService.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
    }

}
