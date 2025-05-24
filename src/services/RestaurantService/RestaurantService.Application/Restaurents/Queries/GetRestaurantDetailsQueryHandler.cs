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

namespace RestaurantService.Application.Restaurents.Queries
{
    public record GetRestaurantDetailsQueryHandler : IRequestHandler<GetRestaurantDetailsQuery, RestaurantDetailsDto>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper;

        public GetRestaurantDetailsQueryHandler(IMapper mapper, IRestaurantRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<RestaurantDetailsDto> Handle(GetRestaurantDetailsQuery request, CancellationToken ct)
        {

            var restaurant = await _repository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null)
            {
               throw new Exception($"Restaurant with ID {request.RestaurantId} not found");
            }
            // If using AutoMapper, you can map directly        
            return _mapper.Map<RestaurantDetailsDto>(restaurant);
        }
    }
}
