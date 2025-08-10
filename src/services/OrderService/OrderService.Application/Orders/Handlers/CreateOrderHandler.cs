using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly IOrderBusinessValidationService _businessValidationService;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(
            IRestaurentService restaurentService, 
            IOrderUnitOfWork unitOfWork, 
            IMapper mapper,
            IOrderBusinessValidationService businessValidationService,
            ILogger<CreateOrderHandler> logger)
        {
            _restaurentService = restaurentService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _businessValidationService = businessValidationService;
            _logger = logger;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating order for user {UserId} at restaurant {RestaurantId}", 
                request.createOrderRequest.UserId, request.createOrderRequest.RestaurantId);

            // Perform comprehensive business validation
            var validationResult = await _businessValidationService.ValidateOrderCreationAsync(request.createOrderRequest);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Order creation validation failed for user {UserId}: {Errors}", 
                    request.createOrderRequest.UserId, string.Join(", ", validationResult.Errors));
                
                throw new OrderBusinessValidationException(validationResult);
            }

            _logger.LogInformation("Order validation passed, proceeding with order creation");

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

            _logger.LogInformation("Order {OrderId} created successfully for user {UserId}", 
                order.OrderId, request.createOrderRequest.UserId);

            return _mapper.Map<OrderDto>(order);
        }
    }
}