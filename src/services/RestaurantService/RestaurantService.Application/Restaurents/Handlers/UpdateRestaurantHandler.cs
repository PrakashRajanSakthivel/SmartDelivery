using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Commands;

namespace RestaurentService.Application.Restaurents.Handlers
{
    public class UpdateRestaurantHandler : IRequestHandler<UpdateRestaurantCommand, bool>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRestaurantHandler> _logger;

        public UpdateRestaurantHandler(
            IRestaurantUnitOfWork unitOfWork,
            ILogger<UpdateRestaurantHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(request.UpdateRestaurantRequest.RestaurantId);

            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant not found: {RestaurantId}", request.UpdateRestaurantRequest.RestaurantId);
                throw new KeyNotFoundException("Restaurant not found");
            }

            // Update restaurant properties
            restaurant.Name = request.UpdateRestaurantRequest.Name;
            restaurant.Description = request.UpdateRestaurantRequest.Description;
            restaurant.Address = request.UpdateRestaurantRequest.Address;
            restaurant.PhoneNumber = request.UpdateRestaurantRequest.PhoneNumber;
            restaurant.Email = request.UpdateRestaurantRequest.Email;
            restaurant.OpeningHours = request.UpdateRestaurantRequest.OpeningHours;
            restaurant.CuisineType = request.UpdateRestaurantRequest.CuisineType;
            restaurant.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Restaurants.UpdateAsync(restaurant);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Restaurant {RestaurantId} updated successfully", restaurant.Id);
            return true;
        }
    }
}