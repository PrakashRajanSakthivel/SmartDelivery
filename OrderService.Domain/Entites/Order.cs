using System;  // Guid is in the System namespace

namespace OrderService.Domain.Entites
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid RestaurantId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<OrderStatusHistory>? StatusHistories { get; set; }
    }

}
