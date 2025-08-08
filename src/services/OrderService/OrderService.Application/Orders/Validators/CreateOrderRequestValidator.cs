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
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.createOrderRequest.UserId).NotEmpty();
            RuleFor(x => x.createOrderRequest.RestaurantId).NotEmpty();
            RuleFor(x => x.createOrderRequest.Items).NotEmpty().WithMessage("Order must have at least one item");
            RuleForEach(x => x.createOrderRequest.Items).SetValidator(new OrderItemRequestValidator());
        }
    }

    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(x => x.MenuItemId).NotEmpty();
            RuleFor(x => x.ItemName).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.UnitPrice).GreaterThan(0);
        }
    }
}
