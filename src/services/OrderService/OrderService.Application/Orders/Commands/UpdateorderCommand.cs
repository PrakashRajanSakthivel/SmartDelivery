using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Orders.DTO;

namespace OrderService.Application.Orders.Commands
{
    public record UpdateOrderCommand(UpdateOrderRequest UpdateOrderRequest) : IRequest<bool>;
}
