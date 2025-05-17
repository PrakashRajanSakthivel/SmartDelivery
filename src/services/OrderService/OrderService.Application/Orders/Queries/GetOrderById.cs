using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace OrderService.Application.Orders.Queries
{
    public record GetOrderById(Guid Id) : IRequest<OrderDto>;
}
