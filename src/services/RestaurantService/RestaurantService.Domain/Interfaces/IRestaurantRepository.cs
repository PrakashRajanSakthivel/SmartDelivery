using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurentService.Domain.Entites;
using Shared.Data.Interfaces;

namespace RestaurentService.Domain.Interfaces
{
    // RestaurantService.Domain/Interfaces/IRestaurantRepository.cs
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<Restaurant> GetRestaurantWithMenuAsync(Guid restaurantId);
        Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string searchTerm);
        Task<IEnumerable<Restaurant>> GetPopularRestaurantsAsync(int count);
        Task AddRestaurantWithMenuAsync(Restaurant restaurant, List<Category> categories, List<MenuItem> menuItems);
    }
}
