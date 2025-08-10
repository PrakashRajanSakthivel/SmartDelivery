using MediatR;
using RestaurantService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;
using RestaurentService.Domain.Entites;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class IsRestaurantActiveQueryHandler : IRequestHandler<IsRestaurantActiveQuery, RestaurantActiveStatusDto>
    {
        private readonly IRestaurantRepository _repository;

        public IsRestaurantActiveQueryHandler(IRestaurantRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<RestaurantActiveStatusDto> Handle(IsRestaurantActiveQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetRestaurantWithMenuAsync(request.RestaurantId);
            
            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant with ID {request.RestaurantId} not found");
            }

            var hasAvailableMenuItems = restaurant.MenuItems.Any(mi => mi.IsAvailable);
            var isAvailableForOrders = restaurant.IsActive && 
                                     restaurant.Status == RestaurantStatus.Active && 
                                     hasAvailableMenuItems;

            var reason = !restaurant.IsActive ? "Restaurant is not active" :
                        restaurant.Status != RestaurantStatus.Active ? $"Restaurant status is {restaurant.Status}" :
                        !hasAvailableMenuItems ? "No available menu items" : null;

            return new RestaurantActiveStatusDto
            {
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                IsActive = restaurant.IsActive,
                Status = restaurant.Status,
                HasAvailableMenuItems = hasAvailableMenuItems,
                IsAvailableForOrders = isAvailableForOrders,
                Reason = reason
            };
        }
    }
}
