using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Orders.Commands;

namespace OrderService.Application.Orders.DTO
{
    public class UpdateOrderRequest
    {
        public Guid OrderId { get; set; }  // Required for update
        public Guid UserId { get; set; }    // For verification
        public List<OrderItemDto> Items { get; set; }

        public string ETag { get; set; }
    }
}
