using MediatR;
using OrderService.Application.Orders.DTO;
using OrderService.Application.Orders.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Commands
{
    public record CreateOrderCommand(CreateOrderRequest createOrderRequest) : IRequest<OrderDto>;
}
