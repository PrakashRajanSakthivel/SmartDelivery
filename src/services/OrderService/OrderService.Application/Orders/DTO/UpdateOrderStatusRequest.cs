using OrderService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.DTO
{
    public class UpdateOrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }
        public string? Reason { get; set; }
    }
}
