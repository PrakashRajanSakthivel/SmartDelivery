using CartService.Application.Commands;
using CartService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Handlers
{
    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, bool>
    {
        private readonly ICartUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCartItemHandler> _logger;

        public UpdateCartItemHandler(ICartUnitOfWork unitOfWork, ILogger<UpdateCartItemHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating item {MenuItemId} quantity to {Quantity} for user {UserId}", 
                request.MenuItemId, request.Quantity, request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);

            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user {UserId}", request.UserId);
                throw new KeyNotFoundException("Cart not found");
            }

            var cartItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, request.MenuItemId);

            if (cartItem == null)
            {
                _logger.LogWarning("Item {MenuItemId} not found in cart {CartId}", 
                    request.MenuItemId, cart.Id);
                throw new KeyNotFoundException("Cart item not found");
            }

            // Update quantity
            cartItem.Quantity = request.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;

            // Update cart timestamp
            cart.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Successfully updated item {MenuItemId} quantity to {Quantity} in cart {CartId}", 
                request.MenuItemId, request.Quantity, cart.Id);

            return true;
        }
    }
} 