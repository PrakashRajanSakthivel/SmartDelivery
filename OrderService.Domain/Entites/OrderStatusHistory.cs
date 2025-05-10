namespace OrderService.Domain.Entites
{
    public class OrderStatusHistory
    {
        public Guid StatusId { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }

        public Order? Order { get; set; }
    }

}
