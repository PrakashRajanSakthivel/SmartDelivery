using FluentValidation;
using CartService.Application;

namespace CartService.Application.Validators
{
    public class UpdateCartItemRequestValidator : AbstractValidator<UpdateCartItemRequest>
    {
        public UpdateCartItemRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(99)
                .WithMessage("Quantity must be between 1 and 99");
        }
    }
} 