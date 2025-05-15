using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Domain.Entites;
using RestaurentService.Domain.Interfaces;

namespace RestaurentService.Application.Restaurents.Handlers
{
    // RestaurantService.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantHandler.cs
    public class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, Guid>
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public CreateRestaurantHandler(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = request.CreateRestaurantRequest.Name,
                Description = request.CreateRestaurantRequest.Description,
                Address = request.CreateRestaurantRequest.Address,
                PhoneNumber = request.CreateRestaurantRequest.PhoneNumber,
                DeliveryFee = request.CreateRestaurantRequest.DeliveryFee,
                MinOrderAmount = request.CreateRestaurantRequest.MinOrderAmount,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _restaurantRepository.AddAsync(restaurant);
            await _restaurantRepository.SaveChangesAsync();

            return restaurant.Id;
        }
    }
}
