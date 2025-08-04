using CartService.Application.Commands;
using CartService.Domain;
using CartService.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Handlers
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartUnitOfWork _unitOfWork;

        public AddCartItemHandler(ICartRepository cartRepository, ICartUnitOfWork cartUnitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = cartUnitOfWork;
        }

        public async Task<CartDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
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
                
            }

            // Check if item already exists
            var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, request.Item.MenuItemId);

            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.Item.Quantity;
                existingItem.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateCartItemAsync(existingItem);
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
            }

            // Update cart timestamp
            cart.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Carts.UpdateAsync(cart);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new CartDto
            {
                Id = cart.Id.ToString(),
                UserId = cart.UserId,
                RestaurantId = cart.RestaurantId,
                Items = cart.Items.Select(item => new CartItemDto
                {
                    Id = item.Id.ToString(),
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItemName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                    ImageUrl = item.ImageUrl
                }).ToList(),
                TotalAmount = cart.TotalAmount,
                TotalItems = cart.TotalItems,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt
            };
        }
    }

}
