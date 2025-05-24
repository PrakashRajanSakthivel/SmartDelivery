using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;
using OrderService.Infra.Repository;

namespace OrderService.Application.Orders.Handlers
{
    // UpdateOrderStatusHandler.cs
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderStatusHandler> _logger;

        public UpdateOrderStatusHandler(IOrderRepository repository, ILogger<UpdateOrderStatusHandler> logger)
        {
            _orderRepository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found", request.OrderId);
                return false;
            }

            // Validate status transition
            if (!IsValidTransition(order.Status, request.NewStatus))
            {
                _logger.LogWarning("Invalid status transition from {CurrentStatus} to {NewStatus}",
                    order.Status, request.NewStatus);
                return false;
            }

            // Update status
            order.Status = request.NewStatus;
            order.UpdatedAt = DateTime.UtcNow;

            // Handle special cases
            switch (request.NewStatus)
            {
                case OrderStatus.Cancelled:
                    order.CancelledAt = DateTime.UtcNow;
                    order.CancellationReason = request.Reason;
                    break;

                case OrderStatus.Delivered:
                    order.DeliveredAt = DateTime.UtcNow;
                    break;
            }

            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} status updated to {Status}",
                order.OrderId, order.Status);

            return true;
        }

        private bool IsValidTransition(OrderStatus current, OrderStatus next)
        {
            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                [OrderStatus.Created] = new()
        {
            OrderStatus.Paid,
            OrderStatus.Cancelled
        },
                [OrderStatus.Paid] = new()
        {
            OrderStatus.Preparing,
            OrderStatus.Cancelled,
            OrderStatus.Refunded  // Added refund option
        },
                [OrderStatus.Preparing] = new()
        {
            OrderStatus.ReadyForDelivery,
            OrderStatus.Cancelled  // Can cancel during preparation
        },
                [OrderStatus.ReadyForDelivery] = new()
        {
            OrderStatus.OutForDelivery,
            OrderStatus.Cancelled  // Rare but possible before dispatch
        },
                [OrderStatus.OutForDelivery] = new()
        {
            OrderStatus.Delivered,
            OrderStatus.Returned  // For failed deliveries
        },
                [OrderStatus.Delivered] = new()
        {
            OrderStatus.Refunded  // Post-delivery refunds
        }
            };

            return allowedTransitions.TryGetValue(current, out var validNext)
                   && validNext.Contains(next);
        }
    }

}
