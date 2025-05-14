namespace OrderService.Application.Model
{
    public class CreateOrderRequest
    {
        public Guid UserId { get; set; }
        public Guid RestaurantId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
