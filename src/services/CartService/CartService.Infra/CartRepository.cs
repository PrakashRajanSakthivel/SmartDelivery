using CartService.Domain;
using CartService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infra
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly CartDbContext _context;

        public CartRepository(CartDbContext context) : base(context) { }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

       

        public async Task<CartItem?> GetCartItemAsync(Guid cartId, string menuItemId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.MenuItemId == menuItemId);
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);

            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);

            return cartItem;
        }

        public async Task<bool> RemoveCartItemAsync(Guid cartId, string menuItemId)
        {
            var cartItem = await GetCartItemAsync(cartId, menuItemId);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);

            return true;
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);

        }
    }
}
