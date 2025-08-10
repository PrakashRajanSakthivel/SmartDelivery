using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Application.Restaurents.Services;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Commands.MenuItem;
using RestaurentService.Application.Restaurents.Commands.Category;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Domain.Entites;

namespace RestaurantService.Application.Restaurents.Services
{
    public class RestaurantBusinessValidationService : IRestaurantBusinessValidationService
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly ILogger<RestaurantBusinessValidationService> _logger;

        public RestaurantBusinessValidationService(
            IRestaurantUnitOfWork unitOfWork,
            ILogger<RestaurantBusinessValidationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ValidationResult> ValidateRestaurantActivationAsync(Guid restaurantId)
        {
            var errors = new List<string>();

            var restaurant = await _unitOfWork.Restaurants.GetRestaurantWithMenuAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add($"Restaurant with ID {restaurantId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Business Rule: Restaurant must have at least one menu item to be active
            if (!restaurant.MenuItems.Any())
            {
                errors.Add($"Restaurant '{restaurant.Name}' cannot be activated because it has no menu items");
            }

            // Check if restaurant has at least one available menu item
            if (!restaurant.MenuItems.Any(mi => mi.IsAvailable))
            {
                errors.Add($"Restaurant '{restaurant.Name}' cannot be activated because it has no available menu items");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateRestaurantDeactivationAsync(Guid restaurantId)
        {
            var errors = new List<string>();

            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add($"Restaurant with ID {restaurantId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Note: Order-related validation should be handled by OrderService
            // RestaurantService should not be responsible for order validation
            // This follows the principle of service independence

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateMenuItemsForRestaurantAsync(Guid restaurantId)
        {
            var errors = new List<string>();

            var restaurant = await _unitOfWork.Restaurants.GetRestaurantWithMenuAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add($"Restaurant with ID {restaurantId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Check if restaurant has at least one menu item
            if (!restaurant.MenuItems.Any())
            {
                errors.Add($"Restaurant '{restaurant.Name}' must have at least one menu item to be active");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateCategoryDeletionAsync(Guid categoryId, Guid restaurantId)
        {
            var errors = new List<string>();

            var category = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                errors.Add($"Category with ID {categoryId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Business Rule: Category cannot be deleted if it has menu items
            var menuItemsInCategory = await _unitOfWork.MenuItems.Where(mi => mi.CategoryId == categoryId).ToListAsync();
            if (menuItemsInCategory.Any())
            {
                errors.Add($"Category '{category.Name}' cannot be deleted because it has {menuItemsInCategory.Count()} menu items");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateMenuItemDeletionAsync(Guid menuItemId, Guid restaurantId)
        {
            var errors = new List<string>();

            var menuItem = await _unitOfWork.MenuItems.FirstOrDefaultAsync(mi => mi.Id == menuItemId);
            if (menuItem == null)
            {
                errors.Add($"Menu item with ID {menuItemId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Note: Order-related validation should be handled by OrderService
            // RestaurantService should not be responsible for order validation
            // This follows the principle of service independence

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateMenuItemCreationAsync(CreateMenuItemRequest request)
        {
            var errors = new List<string>();

            // Business Rule: Menu item name must be unique within the restaurant
            var existingMenuItem = await _unitOfWork.MenuItems.FirstOrDefaultAsync(mi => mi.Name == request.Name && mi.RestaurantId == request.RestaurantId);
            if (existingMenuItem != null)
            {
                errors.Add($"Menu item name '{request.Name}' already exists in this restaurant");
            }

            // Business Rule: Menu item price must be positive
            if (request.Price <= 0)
            {
                errors.Add("Menu item price must be greater than zero");
            }

            // Business Rule: Menu item must belong to a valid category (if category specified)
            if (request.CategoryId.HasValue)
            {
                var category = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId.Value);
                if (category == null)
                {
                    errors.Add($"Category with ID {request.CategoryId.Value} does not exist");
                }
                else if (category.RestaurantId != request.RestaurantId)
                {
                    errors.Add($"Category does not belong to the specified restaurant");
                }
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateMenuItemUpdateAsync(UpdateMenuItemRequest request)
        {
            var errors = new List<string>();

            var existingMenuItem = await _unitOfWork.MenuItems.FirstOrDefaultAsync(mi => mi.Id == request.MenuItemId);
            if (existingMenuItem == null)
            {
                errors.Add($"Menu item with ID {request.MenuItemId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Business Rule: Menu item name must be unique within the restaurant (excluding current item)
            var duplicateMenuItem = await _unitOfWork.MenuItems.FirstOrDefaultAsync(mi => mi.Name == request.Name && mi.RestaurantId == request.RestaurantId);
            if (duplicateMenuItem != null && duplicateMenuItem.Id != request.MenuItemId)
            {
                errors.Add($"Menu item name '{request.Name}' already exists in this restaurant");
            }

            // Business Rule: Menu item price must be positive
            if (request.Price <= 0)
            {
                errors.Add("Menu item price must be greater than zero");
            }

            // Business Rule: Menu item must belong to a valid category (if category specified)
            if (request.CategoryId.HasValue)
            {
                var category = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId.Value);
                if (category == null)
                {
                    errors.Add($"Category with ID {request.CategoryId.Value} does not exist");
                }
                else if (category.RestaurantId != request.RestaurantId)
                {
                    errors.Add($"Category does not belong to the specified restaurant");
                }
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateCategoryCreationAsync(CreateCategoryRequest request)
        {
            var errors = new List<string>();

            // Business Rule: Category name must be unique within the restaurant
            var existingCategory = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Name == request.Name && c.RestaurantId == request.RestaurantId);
            if (existingCategory != null)
            {
                errors.Add($"Category name '{request.Name}' already exists in this restaurant");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }
    }
}
