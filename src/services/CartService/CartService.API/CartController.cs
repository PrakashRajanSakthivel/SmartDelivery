using CartService.Application;
using CartService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.API
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var command = new GetCartCommand { UserId = userId };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("{userId}/items")]
        public async Task<IActionResult> AddItem(string userId, [FromBody] AddCartItemRequest request)
        {
            var command = new AddCartItemCommand
            {
                UserId = userId,
                Item = request
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{userId}/items/{menuItemId}")]
        public async Task<IActionResult> UpdateItem(string userId, string menuItemId, [FromBody] UpdateCartItemRequest request)
        {
            var command = new UpdateCartItemCommand
            {
                UserId = userId,
                MenuItemId = menuItemId,
                Quantity = request.Quantity
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{userId}/items/{menuItemId}")]
        public async Task<IActionResult> RemoveItem(string userId, string menuItemId)
        {
            var command = new RemoveCartItemCommand
            {
                UserId = userId,
                MenuItemId = menuItemId
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var command = new ClearCartCommand { UserId = userId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
