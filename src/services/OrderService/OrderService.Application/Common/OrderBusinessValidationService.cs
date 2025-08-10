using Microsoft.Extensions.Logging;
using OrderService.Application.Orders.DTO;
using OrderService.Domain.Entites;
using RestaurentService.Domain.Entites;

namespace OrderService.Application.Common
{
    public class OrderBusinessValidationService : IOrderBusinessValidationService
    {
        private readonly IRestaurentService _restaurantService;
        private readonly ILogger<OrderBusinessValidationService> _logger;

        public OrderBusinessValidationService(
            IRestaurentService restaurantService,
            ILogger<OrderBusinessValidationService> logger)
        {
            _restaurantService = restaurantService;
            _logger = logger;
        }

        public async Task<ValidationResult> ValidateRestaurantForOrderAsync(Guid restaurantId)
        {
            var errors = new List<string>();

            // Get restaurant validation data in a single call
            var restaurant = await _restaurantService.GetRestaurantForValidationAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add($"Restaurant with ID {restaurantId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Check if restaurant is active
            if (!restaurant.IsActive)
            {
                errors.Add($"Restaurant '{restaurant.Name}' is not active");
            }

            // Check restaurant status
            if (restaurant.Status != RestaurantStatus.Active)
            {
                errors.Add($"Restaurant '{restaurant.Name}' is not available for orders. Current status: {restaurant.Status}");
            }

            // Check if restaurant has any available menu items
            if (!restaurant.MenuItems.Any(mi => mi.IsAvailable))
            {
                errors.Add($"Restaurant '{restaurant.Name}' has no available menu items");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateMenuItemsAsync(Guid restaurantId, List<OrderItemRequest> orderItems)
        {
            var errors = new List<string>();

            if (orderItems == null || !orderItems.Any())
            {
                errors.Add("Order must contain at least one item");
                return ValidationResult.Failure(errors);
            }

            // Get restaurant validation data (cached from previous call if available)
            var restaurant = await _restaurantService.GetRestaurantForValidationAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add($"Restaurant with ID {restaurantId} does not exist");
                return ValidationResult.Failure(errors);
            }

            var menuItemDict = restaurant.MenuItems.ToDictionary(mi => mi.Id, mi => mi);

            foreach (var orderItem in orderItems)
            {
                // Check if menu item exists
                if (!menuItemDict.ContainsKey(orderItem.MenuItemId))
                {
                    errors.Add($"Menu item with ID {orderItem.MenuItemId} does not exist in this restaurant");
                    continue;
                }

                var menuItem = menuItemDict[orderItem.MenuItemId];

                // Check if menu item belongs to the restaurant
                if (menuItem.RestaurantId != restaurantId)
                {
                    errors.Add($"Menu item '{menuItem.Name}' does not belong to the specified restaurant");
                }

                // Check if menu item is available
                if (!menuItem.IsAvailable)
                {
                    errors.Add($"Menu item '{menuItem.Name}' is not available");
                }

                // Check if price matches
                if (menuItem.Price != orderItem.UnitPrice)
                {
                    errors.Add($"Price mismatch for '{menuItem.Name}'. Expected: {menuItem.Price:C}, Provided: {orderItem.UnitPrice:C}");
                }

                // Check quantity limits
                if (orderItem.Quantity <= 0)
                {
                    errors.Add($"Quantity for '{menuItem.Name}' must be greater than 0");
                }
                else if (orderItem.Quantity > 99)
                {
                    errors.Add($"Quantity for '{menuItem.Name}' cannot exceed 99");
                }
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateOrderAmountAsync(Guid restaurantId, decimal orderTotal)
        {
            var errors = new List<string>();

            var restaurant = await _restaurantService.GetRestaurantForValidationAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add("Unable to validate order amount - restaurant not found");
                return ValidationResult.Failure(errors);
            }

            // Check minimum order amount
            if (orderTotal < restaurant.MinOrderAmount)
            {
                errors.Add($"Order total (${orderTotal:F2}) is below the minimum order amount of ${restaurant.MinOrderAmount:F2} for restaurant '{restaurant.Name}'");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateRestaurantOperatingHoursAsync(Guid restaurantId)
        {
            var errors = new List<string>();

            var restaurant = await _restaurantService.GetRestaurantForValidationAsync(restaurantId);
            if (restaurant == null)
            {
                errors.Add("Unable to validate operating hours - restaurant not found");
                return ValidationResult.Failure(errors);
            }

            // Check if restaurant has operating hours configured
            if (string.IsNullOrWhiteSpace(restaurant.OpeningHours))
            {
                _logger.LogWarning("Restaurant {RestaurantId} does not have operating hours configured", restaurantId);
                return ValidationResult.Success(); // Allow if not configured
            }

            // Parse and validate operating hours
            if (!IsRestaurantOpen(restaurant.OpeningHours))
            {
                errors.Add($"Restaurant '{restaurant.Name}' is currently closed. Operating hours: {restaurant.OpeningHours}");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateOrderCreationAsync(CreateOrderRequest request)
        {
            var errors = new List<string>();

            // Get restaurant validation data once and reuse it
            var restaurant = await _restaurantService.GetRestaurantForValidationAsync(request.RestaurantId);
            if (restaurant == null)
            {
                errors.Add($"Restaurant with ID {request.RestaurantId} does not exist");
                return ValidationResult.Failure(errors);
            }

            // Validate restaurant status
            if (!restaurant.IsActive)
            {
                errors.Add($"Restaurant '{restaurant.Name}' is not active");
            }

            if (restaurant.Status != RestaurantStatus.Active)
            {
                errors.Add($"Restaurant '{restaurant.Name}' is not available for orders. Current status: {restaurant.Status}");
            }

            if (!restaurant.MenuItems.Any(mi => mi.IsAvailable))
            {
                errors.Add($"Restaurant '{restaurant.Name}' has no available menu items");
            }

            // Validate menu items
            var menuItemDict = restaurant.MenuItems.ToDictionary(mi => mi.Id, mi => mi);
            foreach (var orderItem in request.Items)
            {
                if (!menuItemDict.ContainsKey(orderItem.MenuItemId))
                {
                    errors.Add($"Menu item with ID {orderItem.MenuItemId} does not exist in this restaurant");
                    continue;
                }

                var menuItem = menuItemDict[orderItem.MenuItemId];

                if (menuItem.RestaurantId != request.RestaurantId)
                {
                    errors.Add($"Menu item '{menuItem.Name}' does not belong to the specified restaurant");
                }

                if (!menuItem.IsAvailable)
                {
                    errors.Add($"Menu item '{menuItem.Name}' is not available");
                }

                if (menuItem.Price != orderItem.UnitPrice)
                {
                    errors.Add($"Price mismatch for '{menuItem.Name}'. Expected: {menuItem.Price:C}, Provided: {orderItem.UnitPrice:C}");
                }

                if (orderItem.Quantity <= 0)
                {
                    errors.Add($"Quantity for '{menuItem.Name}' must be greater than 0");
                }
                else if (orderItem.Quantity > 99)
                {
                    errors.Add($"Quantity for '{menuItem.Name}' cannot exceed 99");
                }
            }

            // Calculate and validate order total
            var orderTotal = request.Items.Sum(item => item.UnitPrice * item.Quantity);
            if (orderTotal < restaurant.MinOrderAmount)
            {
                errors.Add($"Order total (${orderTotal:F2}) is below the minimum order amount of ${restaurant.MinOrderAmount:F2} for restaurant '{restaurant.Name}'");
            }

            // Validate operating hours
            if (!string.IsNullOrWhiteSpace(restaurant.OpeningHours) && !IsRestaurantOpen(restaurant.OpeningHours))
            {
                errors.Add($"Restaurant '{restaurant.Name}' is currently closed. Operating hours: {restaurant.OpeningHours}");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateOrderStatusTransitionAsync(OrderStatus currentStatus, OrderStatus newStatus)
        {
            var errors = new List<string>();

            // Define valid status transitions
            var validTransitions = new Dictionary<OrderStatus, OrderStatus[]>
            {
                { OrderStatus.Created, new[] { OrderStatus.Paid, OrderStatus.Cancelled } },
                { OrderStatus.Paid, new[] { OrderStatus.Preparing, OrderStatus.Cancelled, OrderStatus.Refunded } },
                { OrderStatus.Preparing, new[] { OrderStatus.Ready, OrderStatus.Cancelled } },
                { OrderStatus.Ready, new[] { OrderStatus.OutForDelivery, OrderStatus.Cancelled } },
                { OrderStatus.OutForDelivery, new[] { OrderStatus.Delivered, OrderStatus.Returned } },
                { OrderStatus.Delivered, new[] { OrderStatus.Completed, OrderStatus.Returned } },
                { OrderStatus.Cancelled, new OrderStatus[] { } }, // Terminal state
                { OrderStatus.Refunded, new OrderStatus[] { } }, // Terminal state
                { OrderStatus.Completed, new OrderStatus[] { } }, // Terminal state
                { OrderStatus.Returned, new OrderStatus[] { } }   // Terminal state
            };

            if (!validTransitions.ContainsKey(currentStatus))
            {
                errors.Add($"Invalid current order status: {currentStatus}");
                return ValidationResult.Failure(errors);
            }

            var allowedTransitions = validTransitions[currentStatus];
            if (!allowedTransitions.Contains(newStatus))
            {
                errors.Add($"Invalid status transition from {currentStatus} to {newStatus}");
            }

            return errors.Any() ? ValidationResult.Failure(errors) : ValidationResult.Success();
        }

        private bool IsRestaurantOpen(string openingHours)
        {
            try
            {
                // Simple implementation - can be enhanced with more sophisticated parsing
                // For now, assume restaurant is open if hours are configured
                // This can be expanded to parse actual time ranges
                return !string.IsNullOrWhiteSpace(openingHours);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing restaurant operating hours: {OpeningHours}", openingHours);
                return true; // Default to open if parsing fails
            }
        }
    }
}
