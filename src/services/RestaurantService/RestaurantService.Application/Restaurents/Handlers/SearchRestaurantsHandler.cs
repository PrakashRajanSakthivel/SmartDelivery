using AutoMapper;
using MediatR;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Queries;

namespace RestaurentService.Application.Restaurents.Handlers
{
    public class SearchRestaurantsHandler : IRequestHandler<SearchRestaurantsQuery, List<RestaurantDto>>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchRestaurantsHandler(IRestaurantUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RestaurantDto>> Handle(SearchRestaurantsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                // Return empty list for empty search terms
                return new List<RestaurantDto>();
            }

            var restaurants = await _unitOfWork.Restaurants.SearchRestaurantsAsync(request.SearchTerm);
            
            return _mapper.Map<List<RestaurantDto>>(restaurants);
        }
    }
} 