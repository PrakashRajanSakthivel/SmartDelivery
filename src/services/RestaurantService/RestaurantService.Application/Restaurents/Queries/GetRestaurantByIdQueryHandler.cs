using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;

namespace RestaurantService.Application.Restaurents.Queries
{
    // Application/Queries/GetRestaurantByIdHandler.cs
    public class GetRestaurantByIdHandler : IRequestHandler<GetRestaurantById, RestaurantDto>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper; // AutoMapper (optional)

        public GetRestaurantByIdHandler(IRestaurantRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RestaurantDto> Handle(GetRestaurantById query, CancellationToken ct)
        {
            var restaurant = await _repository.GetByIdAsync(query.Id);
            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found");

            return _mapper.Map<RestaurantDto>(restaurant); // Or manual mapping
        }
    }
}
