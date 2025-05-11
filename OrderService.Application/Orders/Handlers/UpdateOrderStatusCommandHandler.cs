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
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                throw new KeyNotFoundException($"Order with ID {request.OrderId.ToString()} not found.");

            // Business rule: Only allow legal transitions (optional)
            if (!IsValidStatusChange(order.Status, request.NewStatus))
                throw new ApplicationException($"Invalid status change from {order.Status} to {request.NewStatus}");

            order.Status = request.NewStatus;
            order.UpdatedAt = DateTime.UtcNow;

            order.StatusHistories ??= new List<OrderStatusHistory>();
            order.StatusHistories.Add(new OrderStatusHistory
            {
                Id = Guid.NewGuid(),
                OrderId = order.OrderId,
                Status = request.NewStatus,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = request.ChangedBy,
                Note = request.Note
            });

            //await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }


        private bool IsValidStatusChange(OrderStatus current, OrderStatus next)
        {
            // Optional: Add logic here to prevent illegal transitions
            return true;
        }
    }

}
