using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;

// UpdateOrderHandler.cs
namespace OrderService.Application.Orders.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(
            IOrderRepository orderRepository,
            ILogger<UpdateOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(request.UpdateOrderRequest.OrderId);

            if (existingOrder == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", request.UpdateOrderRequest.OrderId);
                throw new KeyNotFoundException("Order not found");
            }

            if (existingOrder.UserId != request.UpdateOrderRequest.UserId)
            {
                _logger.LogWarning("User {UserId} unauthorized to update order {OrderId}",
                    request.UpdateOrderRequest.UserId, request.UpdateOrderRequest.OrderId);
                throw new UnauthorizedAccessException();
            }

            //if (existingOrder.ETag != request.UpdateOrderRequest.ETag)
            //{
            //    throw new DbUpdateConcurrencyException();
            //}

            // Update the order items
            existingOrder.OrderItems = request.UpdateOrderRequest.Items
                .Select(item => new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    MenuItemId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                }).ToList();

            // You might want to update status or other fields
            existingOrder.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(existingOrder);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} updated successfully", existingOrder.OrderId);
            return true;
        }
    }
}
