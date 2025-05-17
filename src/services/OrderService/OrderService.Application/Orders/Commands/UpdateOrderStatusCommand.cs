using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Domain.Entites;

namespace OrderService.Application.Orders.Commands
{
    public class UpdateOrderStatusCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string ChangedBy { get; set; } = "system"; // or "user", "restaurant", etc.
        public string? Note { get; set; }
    }
}
