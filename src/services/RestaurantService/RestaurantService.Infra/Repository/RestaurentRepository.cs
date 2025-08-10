using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurentService.Domain.Entites;
using RestaurentService.Domain.Interfaces;
using RestaurentService.Infra.Data;
using Shared.Data.Repositories;

namespace RestaurentService.Infra.Repository
{
    // RestaurantService.Infra.Data/Repositories/RestaurantRepository.cs
    public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
    {
        private readonly RestaurantDbContext _context;

        public RestaurantRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Restaurant> GetRestaurantWithMenuAsync(Guid restaurantId)
        {
            return await _context.Restaurants
                .Include(r => r.MenuItems)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);
        }

        public async Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string searchTerm)
        {
            return await _context.Restaurants
                .Where(r => r.Name.Contains(searchTerm) ||
                       r.Description.Contains(searchTerm) ||
                       r.MenuItems.Any(m => m.Name.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetPopularRestaurantsAsync(int count)
        {
            return await _context.Restaurants
                .OrderByDescending(r => r.AverageRating)
                .ThenByDescending(r => r.MenuItems.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task AddRestaurantWithMenuAsync(
    Restaurant restaurant,
    List<Category> categories,
    List<MenuItem> menuItems)
        {
            // EF Core tracks all changes in single UoW
            await _context.Restaurants.AddAsync(restaurant);
            await _context.Categories.AddRangeAsync(categories);
            await _context.MenuItems.AddRangeAsync(menuItems);

            // SaveChangesAsync() will commit all as one transaction
        }

    }
}
