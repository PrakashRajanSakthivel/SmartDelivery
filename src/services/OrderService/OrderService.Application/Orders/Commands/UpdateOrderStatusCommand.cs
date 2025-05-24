using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Domain.Entites;

namespace OrderService.Application.Orders.Commands
{
    public sealed record UpdateOrderStatusCommand(
        Guid OrderId,
        OrderStatus NewStatus,
        string? Reason = null) : IRequest<bool>
    {
        public object ChangedBy { get; internal set; }
    }
}
