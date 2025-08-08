using AutoMapper;
using MediatR;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Handlers
{
    public class GetActiveRestaurantsHandler : IRequestHandler<GetActiveRestaurantsQuery, List<RestaurantDto>>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveRestaurantsHandler(IRestaurantUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RestaurantDto>> Handle(GetActiveRestaurantsQuery request, CancellationToken cancellationToken)
        {
            var restaurants = await _unitOfWork.Restaurants.GetAllAsync();
            var activeRestaurants = restaurants.Where(r => r.Status == RestaurantStatus.Active).ToList();
            
            return _mapper.Map<List<RestaurantDto>>(activeRestaurants);
        }
    }
} 