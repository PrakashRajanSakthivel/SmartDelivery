using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurentService.Domain.Entites;
using RestaurentService.Infra.Data;
using Shared.Data.Interfaces;
using Shared.Data.Repositories;

namespace RestaurentService.Infra.Repository
{
    // RestaurantService.Infra.Data/Repositories/RestaurantRepository.cs
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<Restaurant> GetWithMenuAsync(Guid restaurantId);
        Task<IEnumerable<Restaurant>> SearchAsync(string query, int maxResults);
        // Other restaurant-specific queries
    }

    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        private readonly RestaurantDbContext _context;

        public RestaurantRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Restaurant> GetWithMenuAsync(Guid restaurantId)
        {
            return await _context.Restaurants
                .Include(r => r.MenuItems)
                .Include(r => r.Categories)
                    .ThenInclude(c => c.MenuItems)
                .FirstOrDefaultAsync(r => r.Id == restaurantId);
        }

        public async Task<IEnumerable<Restaurant>> SearchAsync(string query, int maxResults)
        {
            return await _context.Restaurants
                .Where(r => r.Name.Contains(query) ||
                       r.Description.Contains(query) ||
                       r.MenuItems.Any(m => m.Name.Contains(query)))
                .Take(maxResults)
                .ToListAsync();
        }
    }
}
