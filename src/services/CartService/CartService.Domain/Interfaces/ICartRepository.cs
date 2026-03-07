using Shared.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        // Keep only custom methods:
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<CartItem?> GetCartItemAsync(Guid cartId, string menuItemId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> RemoveCartItemAsync(Guid cartId, string menuItemId);
        Task ClearCartAsync(Guid cartId);
    }
}
