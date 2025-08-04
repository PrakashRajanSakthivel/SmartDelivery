using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Orders.Commands;
using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Orders.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(
            ILogger<UpdateOrderHandler> logger,
            IOrderUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(request.UpdateOrderRequest.OrderId);

            if (existingOrder == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", request.UpdateOrderRequest.OrderId);
                throw new KeyNotFoundException("Order not found");
            }

            if (existingOrder.UserId != request.UpdateOrderRequest.UserId)
            {
                _logger.LogWarning("User {UserId} unauthorized to update order {OrderId}",
                    request.UpdateOrderRequest.UserId, request.UpdateOrderRequest.OrderId);
                throw new UnauthorizedAccessException("User not authorized to update this order");
            }

            // Only update order properties, not navigation properties
            existingOrder.TotalAmount = request.UpdateOrderRequest.Items.Sum(item => item.UnitPrice * item.Quantity);
            existingOrder.UpdatedAt = DateTime.UtcNow;
            existingOrder.Notes = request.UpdateOrderRequest.Notes;

            // Don't touch OrderItems at all
            await _unitOfWork.Orders.UpdateAsync(existingOrder);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Order {OrderId} updated successfully", existingOrder.OrderId);
            return true;
        }
    }
}