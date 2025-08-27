using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OrderService.Application.Common;
using OrderService.Application.Orders.Commands;
using OrderService.Application.Orders.Queries;
using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Orders.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IRestaurentService _restaurentService;
        private readonly IOrderUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IRestaurentService restaurentService, IOrderUnitOfWork unitOfWork, IMapper mapper)
        {
            _restaurentService = restaurentService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //if (!await _restaurentService.IsPresent(request.createOrderRequest.RestaurantId))
            //{
            //    throw new Exception("Restaurant not found");
            //}

            var now = DateTime.UtcNow;
            var orderItems = request.createOrderRequest.Items.Select(item => new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                MenuItemId = item.MenuItemId,
                ItemName = item.ItemName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.UnitPrice * item.Quantity
            }).ToList();

            var totalAmount = orderItems.Sum(item => item.TotalPrice);

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = request.createOrderRequest.UserId,
                RestaurantId = request.createOrderRequest.RestaurantId,
                Status = OrderStatus.Created,
                TotalAmount = totalAmount,
                CreatedAt = now,
                UpdatedAt = now,
                DeliveredAt = now, // This should be set when actually delivered
                Notes = request.createOrderRequest.Notes,
                OrderItems = orderItems
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<OrderDto>(order);
        }
    }
}