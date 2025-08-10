using FluentValidation;
using RestaurentService.Application.Restaurents.Commands;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Validators
{
    public class UpdateRestaurantStatusRequestValidator : AbstractValidator<UpdateRestaurantStatusRequest>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;

        public UpdateRestaurantStatusRequestValidator(IRestaurantUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.RestaurantId).NotEmpty();
            RuleFor(x => x.NewStatus).IsInEnum();
            RuleFor(x => x.Reason).MaximumLength(500);

            // Business Rule: Restaurant status changes must follow valid transitions
            RuleFor(x => x)
                .MustAsync(ValidateStatusTransitionAsync)
                .WithMessage("Invalid status transition. Please check the allowed transitions for the current status.");
        }

        private async Task<bool> ValidateStatusTransitionAsync(UpdateRestaurantStatusRequest request, CancellationToken cancellationToken)
        {
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(request.RestaurantId);
            if (restaurant == null)
                return false;

            var currentStatus = restaurant.Status;
            var newStatus = request.NewStatus;

            // Define valid status transitions
            var validTransitions = new Dictionary<RestaurantStatus, RestaurantStatus[]>
            {
                { RestaurantStatus.Pending, new[] { RestaurantStatus.Active, RestaurantStatus.Closed } },
                { RestaurantStatus.Active, new[] { RestaurantStatus.Inactive, RestaurantStatus.Suspended, RestaurantStatus.Closed } },
                { RestaurantStatus.Inactive, new[] { RestaurantStatus.Active, RestaurantStatus.Closed } },
                { RestaurantStatus.Suspended, new[] { RestaurantStatus.Active, RestaurantStatus.Closed } },
                { RestaurantStatus.UnderReview, new[] { RestaurantStatus.Active, RestaurantStatus.Closed } },
                { RestaurantStatus.Closed, new RestaurantStatus[] { } } // Terminal state
            };

            if (!validTransitions.ContainsKey(currentStatus))
                return false;

            return validTransitions[currentStatus].Contains(newStatus);
        }
    }
} 