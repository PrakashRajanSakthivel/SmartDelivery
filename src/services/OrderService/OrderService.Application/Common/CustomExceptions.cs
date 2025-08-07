using OrderService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Common
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(Guid orderId)
            : base($"Order with ID {orderId} was not found") { }
    }

    public class InvalidOrderStatusTransitionException : Exception
    {
        public InvalidOrderStatusTransitionException(OrderStatus from, OrderStatus to)
            : base($"Invalid status transition from {from} to {to}") { }
    }
}
