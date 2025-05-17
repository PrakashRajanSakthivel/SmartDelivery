using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Common;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Orders.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurentService _restaurentService;

        public CreateOrderHandler(IOrderRepository orderRepository, IRestaurentService restaurentService)
        {
            _orderRepository = orderRepository;
            _restaurentService = restaurentService;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //if (!await _restaurentService.IsPresent(request.createOrderRequest.RestaurantId))
            //{
            //    throw new Exception("Restaurant not found");
            //}

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = request.createOrderRequest.UserId,
                RestaurantId = request.createOrderRequest.RestaurantId,
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow,
                OrderItems = request.createOrderRequest.Items.Select(item => new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    MenuItemId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order.OrderId;
        }
    }
}
