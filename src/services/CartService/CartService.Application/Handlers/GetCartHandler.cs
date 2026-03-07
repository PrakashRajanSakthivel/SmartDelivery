using CartService.Application.Commands;
using CartService.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Handlers
{
    public class GetCartHandler : IRequestHandler<GetCartCommand, CartDto?>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartDto?> Handle(GetCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserIdAsync(request.UserId);

            if (cart == null)
                return null;

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
