using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Commands.MenuItem;
using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Handlers.MenuItem
{
    public class CreateMenuItemHandler : IRequestHandler<CreateMenuItemCommand, Guid>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly ILogger<CreateMenuItemHandler> _logger;

        public CreateMenuItemHandler(
            IRestaurantUnitOfWork unitOfWork,
            ILogger<CreateMenuItemHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            // Verify restaurant exists
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(request.CreateMenuItemRequest.RestaurantId);
            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant not found: {RestaurantId}", request.CreateMenuItemRequest.RestaurantId);
                throw new KeyNotFoundException("Restaurant not found");
            }

            // Verify category exists if provided
            if (request.CreateMenuItemRequest.CategoryId.HasValue)
            {
                var category = await _unitOfWork.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.CreateMenuItemRequest.CategoryId.Value);
                if (category == null)
                {
                    _logger.LogWarning("Category not found: {CategoryId}", request.CreateMenuItemRequest.CategoryId);
                    throw new KeyNotFoundException("Category not found");
                }
            }

            var menuItem = new RestaurentService.Domain.Entites.MenuItem
            {
                Id = Guid.NewGuid(),
                RestaurantId = request.CreateMenuItemRequest.RestaurantId,
                CategoryId = request.CreateMenuItemRequest.CategoryId,
                Name = request.CreateMenuItemRequest.Name,
                Description = request.CreateMenuItemRequest.Description,
                Price = request.CreateMenuItemRequest.Price,
                IsVegetarian = request.CreateMenuItemRequest.IsVegetarian,
                IsVegan = request.CreateMenuItemRequest.IsVegan,
                ImageUrl = request.CreateMenuItemRequest.ImageUrl,
                PreparationTime = request.CreateMenuItemRequest.PreparationTime,
                IsAvailable = request.CreateMenuItemRequest.IsAvailable,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.MenuItems.Add(menuItem);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Menu item {MenuItemId} created for restaurant {RestaurantId}", 
                menuItem.Id, menuItem.RestaurantId);

            return menuItem.Id;
        }
    }
} 