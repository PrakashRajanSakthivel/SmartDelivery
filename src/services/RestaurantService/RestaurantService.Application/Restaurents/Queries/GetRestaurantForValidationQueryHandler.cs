using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;

namespace RestaurantService.Application.Restaurents.Queries
{
    public class GetRestaurantForValidationQueryHandler : IRequestHandler<GetRestaurantForValidationQuery, RestaurantValidationDto>
    {
        private readonly IRestaurantRepository _repository;
        private readonly IMapper _mapper;

        public GetRestaurantForValidationQueryHandler(IRestaurantRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<RestaurantValidationDto> Handle(GetRestaurantForValidationQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _repository.GetRestaurantWithMenuAsync(request.RestaurantId);
            
            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant with ID {request.RestaurantId} not found");
            }

            var validationDto = new RestaurantValidationDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                IsActive = restaurant.IsActive,
                Status = restaurant.Status,
                MinOrderAmount = restaurant.MinOrderAmount,
                DeliveryFee = restaurant.DeliveryFee,
                OpeningHours = restaurant.OpeningHours,
                MenuItems = restaurant.MenuItems.Select(mi => new MenuItemValidationDto
                {
                    Id = mi.Id,
                    RestaurantId = restaurant.Id,
                    Name = mi.Name,
                    Price = mi.Price,
                    IsAvailable = mi.IsAvailable
                }).ToList()
            };

            return validationDto;
        }
    }
}
