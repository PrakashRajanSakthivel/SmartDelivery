using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Orders.Queries;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Orders.QueriesHandlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderById, OrderDto>
    {
        private readonly IOrderRepository _repository;

        public GetOrderByIdHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderDto> Handle(GetOrderById query, CancellationToken ct)
        {
            var order = await _repository.GetByIdAsync(query.Id);
            if (order == null)
                throw new KeyNotFoundException("Restaurant not found");

            return new OrderDto(order.OrderId);
        }
    }
}
