using OrderService.Application.Orders.DTO;
using OrderService.Domain.Entites;

namespace OrderService.Application.Common
{
    public interface IOrderBusinessValidationService
    {
        /// <summary>
        /// Validates restaurant is active and available for orders
        /// </summary>
        Task<ValidationResult> ValidateRestaurantForOrderAsync(Guid restaurantId);

        /// <summary>
        /// Validates all menu items in the order are available and belong to the restaurant
        /// </summary>
        Task<ValidationResult> ValidateMenuItemsAsync(Guid restaurantId, List<OrderItemRequest> orderItems);

        /// <summary>
        /// Validates order meets minimum amount requirements
        /// </summary>
        Task<ValidationResult> ValidateOrderAmountAsync(Guid restaurantId, decimal orderTotal);

        /// <summary>
        /// Validates restaurant operating hours
        /// </summary>
        Task<ValidationResult> ValidateRestaurantOperatingHoursAsync(Guid restaurantId);

        /// <summary>
        /// Comprehensive validation for order creation
        /// </summary>
        Task<ValidationResult> ValidateOrderCreationAsync(CreateOrderRequest request);

        /// <summary>
        /// Validates order status transition
        /// </summary>
        Task<ValidationResult> ValidateOrderStatusTransitionAsync(OrderStatus currentStatus, OrderStatus newStatus);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        public static ValidationResult Success()
        {
            return new ValidationResult { IsValid = true };
        }

        public static ValidationResult Failure(params string[] errors)
        {
            return new ValidationResult 
            { 
                IsValid = false, 
                Errors = errors.ToList() 
            };
        }

        public static ValidationResult Failure(List<string> errors)
        {
            return new ValidationResult 
            { 
                IsValid = false, 
                Errors = errors 
            };
        }
    }
}
