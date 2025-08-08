using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Commands.MenuItem;

namespace RestaurentService.Application.Restaurents.Handlers.MenuItem
{
    public class UpdateMenuItemHandler : IRequestHandler<UpdateMenuItemCommand, bool>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateMenuItemHandler> _logger;

        public UpdateMenuItemHandler(
            IRestaurantUnitOfWork unitOfWork,
            ILogger<UpdateMenuItemHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _unitOfWork.MenuItems
                .FirstOrDefaultAsync(m => m.Id == request.UpdateMenuItemRequest.MenuItemId, cancellationToken);

            if (menuItem == null)
            {
                _logger.LogWarning("Menu item not found: {MenuItemId}", request.UpdateMenuItemRequest.MenuItemId);
                throw new KeyNotFoundException("Menu item not found");
            }

            // Verify restaurant ownership
            if (menuItem.RestaurantId != request.UpdateMenuItemRequest.RestaurantId)
            {
                _logger.LogWarning("Menu item {MenuItemId} does not belong to restaurant {RestaurantId}", 
                    request.UpdateMenuItemRequest.MenuItemId, request.UpdateMenuItemRequest.RestaurantId);
                throw new UnauthorizedAccessException("Menu item does not belong to this restaurant");
            }

            // Update properties
            menuItem.CategoryId = request.UpdateMenuItemRequest.CategoryId;
            menuItem.Name = request.UpdateMenuItemRequest.Name;
            menuItem.Description = request.UpdateMenuItemRequest.Description;
            menuItem.Price = request.UpdateMenuItemRequest.Price;
            menuItem.IsVegetarian = request.UpdateMenuItemRequest.IsVegetarian;
            menuItem.IsVegan = request.UpdateMenuItemRequest.IsVegan;
            menuItem.ImageUrl = request.UpdateMenuItemRequest.ImageUrl;
            menuItem.PreparationTime = request.UpdateMenuItemRequest.PreparationTime;
            menuItem.IsAvailable = request.UpdateMenuItemRequest.IsAvailable;

            // EF Core change tracking will automatically detect these changes
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Menu item {MenuItemId} updated successfully", menuItem.Id);
            return true;
        }
    }
} 