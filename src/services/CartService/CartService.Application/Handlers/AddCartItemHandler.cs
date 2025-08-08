using CartService.Application.Commands;
using CartService.Domain;
using CartService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace CartService.Application.Handlers
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartDto>
    {
        private readonly ICartUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCartItemHandler> _logger;

        public AddCartItemHandler(ICartUnitOfWork unitOfWork, IMapper mapper, ILogger<AddCartItemHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding item to cart for user {UserId}", request.UserId);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);

            if (cart == null)
            {
                // Create new cart
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    RestaurantId = string.Empty, // Will be set when first item is added
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Carts.AddAsync(cart);
                _logger.LogInformation("Created new cart {CartId} for user {UserId}", cart.Id, request.UserId);
            }

            // Check if item already exists
            var existingItem = await _unitOfWork.Carts.GetCartItemAsync(cart.Id, request.Item.MenuItemId);

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.Item.Quantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
                _logger.LogInformation("Updated existing item {MenuItemId} quantity to {Quantity}", 
                    request.Item.MenuItemId, existingItem.Quantity);
            }
            else
            {
                // Add new item
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    MenuItemId = request.Item.MenuItemId,
                    MenuItemName = request.Item.MenuItemName,
                    Quantity = request.Item.Quantity,
                    UnitPrice = request.Item.UnitPrice,
                    ImageUrl = request.Item.ImageUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Carts.AddCartItemAsync(cartItem);
                _logger.LogInformation("Added new item {MenuItemId} to cart {CartId}", 
                    request.Item.MenuItemId, cart.Id);
            }

            // Update cart timestamp
            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.CommitAsync(cancellationToken);

            // Reload cart with items for response
            cart = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId);

            return _mapper.Map<CartDto>(cart);
        }
    }

}
