using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.Queries;
using OrderService.Domain.Interfaces;
using AutoMapper;

namespace OrderService.Application.Orders.Queries
{
    public class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserId, IEnumerable<OrderDto>>
    {
        private readonly IOrderUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrdersByUserIdHandler(IOrderUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserId request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.GetByUserIdAsync(request.UserId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }
    }
}
