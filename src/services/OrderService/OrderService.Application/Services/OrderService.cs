using Microsoft.Extensions.Logging;
using OrderService.Application.Common;
using OrderService.Application.Model;
using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;
using Serilog;

namespace OrderService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderUnitOfWork _orderuow;
        private readonly ILogger<OrderService> _logger;
        private readonly IRestaurentService _restaurentService;

        public OrderService(IOrderUnitOfWork orderuow,  ILogger<OrderService> logger, IRestaurentService restaurentService)
        {
            _orderuow = orderuow;
            _logger = logger;
            _restaurentService = restaurentService;
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderRequest request)
        {
            _logger.LogInformation("Creating order for UserId: {UserId} at {Time}", request.UserId, DateTime.UtcNow);

            if(!await _restaurentService.IsPresent(request.RestaurantId))
            {
                _logger.LogInformation("Restaurant {RestaurantId} is not present", request.RestaurantId);
                throw new Exception("Restaurant not found");
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow,
                OrderItems = request.Items.Select(item => new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    MenuItemId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                }).ToList()
            };

            await _orderuow.Orders.AddAsync(order);
            await _orderuow.Orders.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} created successfully", order.OrderId);
            return order.OrderId;
        }
    }

}
