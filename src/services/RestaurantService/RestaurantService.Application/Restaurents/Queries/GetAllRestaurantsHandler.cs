using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class GetAllRestaurantsHandler : IRequestHandler<GetAllRestaurantsQuery, List<RestaurantDto>>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper; // AutoMapper (optional)

        public GetAllRestaurantsHandler(IRestaurantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken ct)
        {
            var restaurant = await _repository.GetAllAsync();
            if (restaurant == null || !restaurant.Any())
            {
                return new List<RestaurantDto>();
            }
            // If using AutoMapper, you can map directly        
            return _mapper.Map<List<RestaurantDto>>(restaurant);

        }
    }
}
