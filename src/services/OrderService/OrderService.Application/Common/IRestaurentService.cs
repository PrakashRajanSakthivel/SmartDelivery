using OrderService.Application.Orders.DTO;

namespace OrderService.Application.Common
{
    public interface IRestaurentService
    {
        /// <summary>
        /// Get restaurant validation data in a single optimized call
        /// </summary>
        Task<RestaurantValidationDto?> GetRestaurantForValidationAsync(Guid restaurantId);
    }
}
