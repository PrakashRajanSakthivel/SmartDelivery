using RestaurentService.Domain.Entites;
using Shared.Data.Interfaces;

namespace RestaurentService.Domain.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<List<MenuItem>> GetMenuItemsByRestaurantAsync(Guid restaurantId);
        Task<List<MenuItem>> GetMenuItemsByCategoryAsync(Guid categoryId);
        Task<MenuItem?> GetMenuItemByNameAndRestaurantAsync(string name, Guid restaurantId);
        Task<List<MenuItem>> GetAvailableMenuItemsByRestaurantAsync(Guid restaurantId);
    }
}
