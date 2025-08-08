using CartService.Application.Commands;
using CartService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Handlers
{
    public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand, bool>
    {
        private readonly ICartUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveCartItemHandler> _logger;

        public RemoveCartItemHandler(ICartUnitOfWork unitOfWork, ILogger<RemoveCartItemHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing item {MenuItemId} from cart for user {UserId}", 
                request.MenuItemId, request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);

            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user {UserId}", request.UserId);
                throw new KeyNotFoundException("Cart not found");
            }

            var result = await _unitOfWork.Carts.RemoveCartItemAsync(cart.Id, request.MenuItemId);

            if (result)
            {
                // Update cart timestamp
                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.CommitAsync(cancellationToken);
                
                _logger.LogInformation("Successfully removed item {MenuItemId} from cart {CartId}", 
                    request.MenuItemId, cart.Id);
            }
            else
            {
                _logger.LogWarning("Item {MenuItemId} not found in cart {CartId}", 
                    request.MenuItemId, cart.Id);
            }

            return result;
        }
    }
} 