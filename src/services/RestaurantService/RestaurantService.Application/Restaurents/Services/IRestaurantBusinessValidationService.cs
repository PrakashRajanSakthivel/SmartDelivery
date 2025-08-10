using RestaurentService.Domain.Entites;
using RestaurentService.Application.Restaurents.Commands.MenuItem;
using RestaurentService.Application.Restaurents.Commands.Category;

namespace RestaurantService.Application.Restaurents.Services
{
    public interface IRestaurantBusinessValidationService
    {
        Task<ValidationResult> ValidateRestaurantActivationAsync(Guid restaurantId);
        Task<ValidationResult> ValidateRestaurantDeactivationAsync(Guid restaurantId);
        Task<ValidationResult> ValidateMenuItemsForRestaurantAsync(Guid restaurantId);
        Task<ValidationResult> ValidateCategoryDeletionAsync(Guid categoryId, Guid restaurantId);
        Task<ValidationResult> ValidateMenuItemDeletionAsync(Guid menuItemId, Guid restaurantId);
        Task<ValidationResult> ValidateMenuItemCreationAsync(CreateMenuItemRequest request);
        Task<ValidationResult> ValidateMenuItemUpdateAsync(UpdateMenuItemRequest request);
        Task<ValidationResult> ValidateCategoryCreationAsync(CreateCategoryRequest request);
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        public static ValidationResult Success() => new ValidationResult { IsValid = true };
        public static ValidationResult Failure(params string[] errors) => new ValidationResult 
        { 
            IsValid = false, 
            Errors = errors.ToList() 
        };
        public static ValidationResult Failure(List<string> errors) => new ValidationResult 
        { 
            IsValid = false, 
            Errors = errors 
        };
    }
}
