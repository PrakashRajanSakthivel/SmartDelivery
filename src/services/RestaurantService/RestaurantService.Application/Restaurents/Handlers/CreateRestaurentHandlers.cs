using MediatR;
using RestaurantService.Domain.Interfaces;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Domain.Entites;
using RestaurentService.Domain.Interfaces;

namespace RestaurentService.Application.Restaurents.Handlers
{
    // RestaurantService.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantHandler.cs
    public class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, Guid>
    {
        private readonly IRestaurantUnitOfWork _unitOfWork;

        public CreateRestaurantHandler(IRestaurantUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            // 2. Add Categories and create mapping from category index to new CategoryId
            var categoryMapping = new Dictionary<int, Guid>();
            var categories = request.CreateRestaurantRequest.Categories
                .Select((c, index) => 
                {
                    var newCategoryId = Guid.NewGuid();
                    categoryMapping[index] = newCategoryId; // Map index to new ID
                    
                    return new Category
                    {
                        Id = newCategoryId,
                        RestaurantId = restaurant.Id,
                        Name = c.Name,
                        DisplayOrder = c.DisplayOrder
                    };
                }).ToList();

            // 3. Add Menu Items with mapped CategoryIds
            var menuItems = request.CreateRestaurantRequest.MenuItems
                .Select(m => new RestaurentService.Domain.Entites.MenuItem
                {
                    Id = Guid.NewGuid(),
                    RestaurantId = restaurant.Id,
                    CategoryId = m.CategoryIndex.HasValue ? categoryMapping[m.CategoryIndex.Value] : null,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    IsVegetarian = m.IsVegetarian,
                    IsVegan = m.IsVegan,
                    PreparationTime = m.PreparationTime,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            // 4. Persist all together using Unit of Work pattern
            await _unitOfWork.Restaurants.AddAsync(restaurant);
            await _unitOfWork.Categories.AddRangeAsync(categories);
            await _unitOfWork.MenuItems.AddRangeAsync(menuItems);
            await _unitOfWork.CommitAsync();

            return restaurant.Id;
        }
    }
}
