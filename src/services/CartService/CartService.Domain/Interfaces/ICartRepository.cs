using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(string userId);
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
        Task<bool> DeleteAsync(Guid cartId);
        Task<CartItem?> GetCartItemAsync(Guid cartId, string menuItemId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task<bool> RemoveCartItemAsync(Guid cartId, string menuItemId);
        Task ClearCartAsync(Guid cartId);
    }
}
