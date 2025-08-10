using MediatR;
using RestaurantService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class GetRestaurantOperatingHoursQueryHandler : IRequestHandler<GetRestaurantOperatingHoursQuery, RestaurantOperatingHoursDto>
    {
        private readonly IRestaurantRepository _repository;

        public GetRestaurantOperatingHoursQueryHandler(IRestaurantRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<RestaurantOperatingHoursDto> Handle(GetRestaurantOperatingHoursQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetByIdAsync(request.RestaurantId);
            
            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant with ID {request.RestaurantId} not found");
            }

            // Simple implementation - can be enhanced with sophisticated time parsing
            var isCurrentlyOpen = !string.IsNullOrWhiteSpace(restaurant.OpeningHours);

            return new RestaurantOperatingHoursDto
            {
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                OpeningHours = restaurant.OpeningHours,
                IsCurrentlyOpen = isCurrentlyOpen,
                NextOpeningTime = null, // Can be implemented with time parsing
                NextClosingTime = null  // Can be implemented with time parsing
            };
        }
    }
}
