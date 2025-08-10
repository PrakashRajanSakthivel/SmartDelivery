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

        public bool IsCancelled { get; set; } = false;
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        public bool IsRefunded { get; set; } = false;
        public DateTime? RefundedAt { get; set; }
        public string? RefundReason { get; set; }

        public string? Notes { get; set; }



        // Navigation property
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<OrderStatusHistory>? StatusHistories { get; set; }
        public DateTime DeliveredAt { get; set; }
    }

}
