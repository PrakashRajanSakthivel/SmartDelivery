using CartService.Application.Commands;
using CartService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CartService.Application.Handlers
{
    public class ClearCartHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly ICartUnitOfWork _unitOfWork;
        private readonly ILogger<ClearCartHandler> _logger;

        public ClearCartHandler(ICartUnitOfWork unitOfWork, ILogger<ClearCartHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Clearing cart for user {UserId}", request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);

            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user {UserId}", request.UserId);
                throw new KeyNotFoundException("Cart not found");
            }

            await _unitOfWork.Carts.ClearCartAsync(cart.Id);

            // Update cart timestamp
            cart.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Successfully cleared cart {CartId} for user {UserId}", 
                cart.Id, request.UserId);

            return true;
        }
    }
} 