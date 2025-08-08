using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Commands;

namespace RestaurentService.Application.Restaurents.Handlers
{
    public class DeleteRestaurantHandler : IRequestHandler<DeleteRestaurantCommand, bool>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRestaurantHandler> _logger;

        public DeleteRestaurantHandler(
            IRestaurantUnitOfWork unitOfWork,
            ILogger<DeleteRestaurantHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(request.RestaurantId);

            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant not found: {RestaurantId}", request.RestaurantId);
                throw new KeyNotFoundException("Restaurant not found");
            }

            // Delete related entities first (cascade delete)
            var menuItems = await _unitOfWork.MenuItems
                .Where(m => m.RestaurantId == request.RestaurantId)
                .ToListAsync(cancellationToken);
            
            var categories = await _unitOfWork.Categories
                .Where(c => c.RestaurantId == request.RestaurantId)
                .ToListAsync(cancellationToken);

            // Remove in correct order to avoid FK constraint violations
            _unitOfWork.MenuItems.RemoveRange(menuItems);
            _unitOfWork.Categories.RemoveRange(categories);
            _unitOfWork.Restaurants.Remove(restaurant);
            
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Restaurant {RestaurantId} and all related entities deleted successfully", request.RestaurantId);
            return true;
        }
    }
} 