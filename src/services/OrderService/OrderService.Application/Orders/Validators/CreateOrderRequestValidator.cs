using FluentValidation;
using OrderService.Application.Orders.Commands;
using OrderService.Application.Orders.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.createOrderRequest.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");

            RuleFor(x => x.createOrderRequest.RestaurantId)
                .NotEmpty()
                .WithMessage("Restaurant ID is required");

            RuleFor(x => x.createOrderRequest.Items)
                .NotEmpty()
                .WithMessage("Order must have at least one item")
                .Must(items => items != null && items.Count <= 50)
                .WithMessage("Order cannot contain more than 50 items");

            RuleForEach(x => x.createOrderRequest.Items)
                .SetValidator(new OrderItemRequestValidator());

            // Validate order total is reasonable
            RuleFor(x => x.createOrderRequest.Items)
                .Must(items => items.Sum(item => item.UnitPrice * item.Quantity) > 0)
                .WithMessage("Order total must be greater than zero");

            RuleFor(x => x.createOrderRequest.Items)
                .Must(items => items.Sum(item => item.UnitPrice * item.Quantity) <= 1000)
                .WithMessage("Order total cannot exceed $1,000");

            // Validate notes length
            RuleFor(x => x.createOrderRequest.Notes)
                .MaximumLength(500)
                .WithMessage("Order notes cannot exceed 500 characters");
        }
    }

    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(x => x.MenuItemId)
                .NotEmpty()
                .WithMessage("Menu item ID is required");

            RuleFor(x => x.ItemName)
                .NotEmpty()
                .WithMessage("Item name is required")
                .MaximumLength(100)
                .WithMessage("Item name cannot exceed 100 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(99)
                .WithMessage("Quantity cannot exceed 99");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Unit price must be greater than 0")
                .LessThanOrEqualTo(500)
                .WithMessage("Unit price cannot exceed $500");

            // Validate total price for this item
            RuleFor(x => x)
                .Must(item => item.UnitPrice * item.Quantity <= 1000)
                .WithMessage("Total price for this item cannot exceed $1,000");
        }
    }
}
