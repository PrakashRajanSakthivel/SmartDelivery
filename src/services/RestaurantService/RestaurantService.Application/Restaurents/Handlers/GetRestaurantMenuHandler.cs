using AutoMapper;
using MediatR;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Queries;

namespace RestaurentService.Application.Restaurents.Handlers
{
    public class GetRestaurantMenuHandler : IRequestHandler<GetRestaurantMenuQuery, RestaurantMenuDto>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRestaurantMenuHandler(IRestaurantUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RestaurantMenuDto> Handle(GetRestaurantMenuQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _unitOfWork.Restaurants.GetRestaurantWithMenuAsync(request.RestaurantId);
            
            if (restaurant == null)
                throw new KeyNotFoundException("Restaurant not found");

            return _mapper.Map<RestaurantMenuDto>(restaurant);
        }
    }
} 