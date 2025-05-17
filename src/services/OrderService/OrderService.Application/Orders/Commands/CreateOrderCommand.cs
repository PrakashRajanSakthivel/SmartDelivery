using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace OrderService.Application.Orders.Commands
{
    public record CreateOrderCommand(CreateOrderRequest createOrderRequest) : IRequest<Guid>;
}
