using AutoMapper;
using MediatR;
using OrderService.Application.Orders.Queries;
using OrderService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.QueriesHandlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderById, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OrderDto> Handle(GetOrderById query, CancellationToken ct)
        {
            var order = await _repository.GetByIdAsync(query.Id);
            if (order == null)
                throw new KeyNotFoundException("Restaurant not found");

            return _mapper.Map<OrderDto>(order);
        }
    }
}
