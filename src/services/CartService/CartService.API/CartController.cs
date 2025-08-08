using CartService.Application;
using CartService.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedSvc.Response;

namespace CartService.API
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : BaseController
    {

        public CartController(IMediator mediator, ILogger<CartController> logger)
                   : base(mediator, logger)
        {
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            var command = new GetCartCommand { UserId = userId };
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("Cart not found");

            return Success(result);
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
            return Success(result, "Item added to cart successfully");
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
            return Success(result, "Cart item updated successfully");
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
            return Success(result, "Item removed from cart successfully");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var command = new ClearCartCommand { UserId = userId };
            var result = await _mediator.Send(command);
            return Success(result, "Cart cleared successfully");
        }
    }
}
