using FluentValidation;
using CartService.Application;

namespace CartService.Application.Validators
{
    public class AddCartItemRequestValidator : AbstractValidator<AddCartItemRequest>
    {
        public AddCartItemRequestValidator()
        {
            RuleFor(x => x.MenuItemId)
                .NotEmpty()
                .WithMessage("MenuItemId is required");

            RuleFor(x => x.MenuItemName)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("MenuItemName is required and must be less than 100 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(99)
                .WithMessage("Quantity must be between 1 and 99");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("UnitPrice must be greater than 0");
        }
    }
} 