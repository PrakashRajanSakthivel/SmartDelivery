using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.Application.Restaurents.Handlers
{
    public class UpdateRestaurantStatusHandler : IRequestHandler<UpdateRestaurantStatusCommand, bool>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRestaurantStatusHandler> _logger;

        public UpdateRestaurantStatusHandler(
            IRestaurantUnitOfWork unitOfWork,
            ILogger<UpdateRestaurantStatusHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateRestaurantStatusCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(request.RestaurantId);

            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant not found: {RestaurantId}", request.RestaurantId);
                throw new KeyNotFoundException("Restaurant not found");
            }
            
            restaurant.Status = request.NewStatus;
            restaurant.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Restaurants.UpdateAsync(restaurant);
            await _unitOfWork.CommitAsync();
            _logger.LogInformation("Restaurant {RestaurantId} status updated to {Status}", request.RestaurantId, request.NewStatus);

            return true;
        }
    }
}
