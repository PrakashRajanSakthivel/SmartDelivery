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

        public async Task<Guid> Handle(CreateRestaurantCommand request, CancellationToken ct)
        {
            // 1. Create Restaurant
            var restaurant = new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = request.CreateRestaurantRequest.Name,
                Description = request.CreateRestaurantRequest.Description,
                Address = request.CreateRestaurantRequest.Address,
                PhoneNumber = request.CreateRestaurantRequest.PhoneNumber,
                DeliveryFee = request.CreateRestaurantRequest.DeliveryFee,
                MinOrderAmount = request.CreateRestaurantRequest.MinOrderAmount,
                CreatedAt = DateTime.UtcNow
            };

            // 2. Add Categories
            var categories = request.CreateRestaurantRequest.Categories
                .Select(c => new Category
                {
                    Id = Guid.NewGuid(),
                    RestaurantId = restaurant.Id,
                    Name = c.Name,
                    DisplayOrder = c.DisplayOrder
                }).ToList();

            // 3. Add Menu Items
            var menuItems = request.CreateRestaurantRequest.MenuItems
                .Select(m => new MenuItem
                {
                    Id = Guid.NewGuid(),
                    RestaurantId = restaurant.Id,
                    CategoryId = m.CategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    IsVegetarian = m.IsVegetarian,
                    IsVegan = m.IsVegan,
                    PreparationTime = m.PreparationTime,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            // 4. Persist all together
            await _restaurantRepository.AddRestaurantWithMenuAsync(restaurant, categories, menuItems);
            await _restaurantRepository.SaveChangesAsync();

            return restaurant.Id;
        }
    }
}
