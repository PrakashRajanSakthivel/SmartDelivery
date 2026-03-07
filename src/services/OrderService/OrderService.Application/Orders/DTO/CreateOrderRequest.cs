using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.DTO
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public Guid RestaurantId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
        public string? Notes { get; set; }
    }

    public class OrderItemRequest
    {
        public Guid MenuItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
