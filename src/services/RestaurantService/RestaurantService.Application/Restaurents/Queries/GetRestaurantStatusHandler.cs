using AutoMapper;
using MediatR;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class GetRestaurantStatusHandler : IRequestHandler<GetRestaurantStatus, RestaurantStatusDto>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public GetRestaurantStatusHandler(IRestaurantUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RestaurantStatusDto> Handle(GetRestaurantStatus query, CancellationToken ct)
        {
            var restaurant = await _unitOfWork.Restaurants.GetByIdAsync(query.Id);
            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found");

            return _mapper.Map<RestaurantStatusDto>(restaurant); 
        }

       
    }
}
