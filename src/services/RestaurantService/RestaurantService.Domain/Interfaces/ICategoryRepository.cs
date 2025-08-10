using RestaurentService.Domain.Entites;
using Shared.Data.Interfaces;

namespace RestaurentService.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetCategoriesByRestaurantAsync(Guid restaurantId);
        Task<Category?> GetCategoryByNameAndRestaurantAsync(string name, Guid restaurantId);
    }
}
