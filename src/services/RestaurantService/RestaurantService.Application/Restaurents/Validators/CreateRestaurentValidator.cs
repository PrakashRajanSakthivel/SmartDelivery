using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RestaurentService.Application.Restaurents.Commands;

namespace RestaurentService.Application.Restaurents.Validators
{
    // RestaurantService.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantValidator.cs
    public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantCommand>
    {
        public CreateRestaurantValidator()
        {
            RuleFor(x => x.CreateRestaurantRequest.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.CreateRestaurantRequest.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.CreateRestaurantRequest.DeliveryFee)
                .GreaterThanOrEqualTo(0).WithMessage("Delivery fee cannot be negative");
        }
    }
}
