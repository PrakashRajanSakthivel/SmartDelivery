using FluentValidation;
using RestaurentService.Application.Restaurents.Commands;

namespace RestaurentService.Application.Restaurents.Validators
{
    public class UpdateRestaurantStatusRequestValidator : AbstractValidator<UpdateRestaurantStatusRequest>
    {
        public UpdateRestaurantStatusRequestValidator()
        {
            RuleFor(x => x.RestaurantId).NotEmpty();
            RuleFor(x => x.NewStatus).IsInEnum();
            RuleFor(x => x.Reason).MaximumLength(500);
        }
    }
} 