using Microsoft.EntityFrameworkCore;
using RestaurentService.Domain.Entites;
using RestaurentService.Domain.Interfaces;

namespace RestaurentService.Infra.Data
{
    public class SampleDataSeeder
    {
        private readonly RestaurantDbContext _context;

        public SampleDataSeeder(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (await _context.Restaurants.AnyAsync())
                return; // Data already seeded

            // Create sample restaurant
            var restaurant = new Restaurant
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
                Name = "Pizza Palace",
                Description = "Best pizza in town",
                Address = "123 Main St, City",
                PhoneNumber = "+1-555-0123",
                Email = "info@pizzapalace.com",
                IsActive = true,
                Status = RestaurantStatus.Active,
                DeliveryFee = 2.99m,
                MinOrderAmount = 10.00m,
                AverageRating = 4.5,
                OpeningHours = "09:00-22:00",
                CuisineType = "Italian",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create sample categories
            var pizzaCategory = new Category
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440010"),
                RestaurantId = restaurant.Id,
                Name = "Pizzas",
                DisplayOrder = 1,
                CreatedAt = DateTime.UtcNow
            };

            var saladCategory = new Category
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440011"),
                RestaurantId = restaurant.Id,
                Name = "Salads",
                DisplayOrder = 2,
                CreatedAt = DateTime.UtcNow
            };

            // Create sample menu items
            var margheritaPizza = new MenuItem
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
                RestaurantId = restaurant.Id,
                CategoryId = pizzaCategory.Id,
                Name = "Margherita Pizza",
                Description = "Classic tomato and mozzarella pizza",
                Price = 15.99m,
                IsAvailable = true,
                IsVegetarian = true,
                PreparationTime = 20,
                CreatedAt = DateTime.UtcNow
            };

            var pepperoniPizza = new MenuItem
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
                RestaurantId = restaurant.Id,
                CategoryId = pizzaCategory.Id,
                Name = "Pepperoni Pizza",
                Description = "Spicy pepperoni with cheese",
                Price = 17.99m,
                IsAvailable = true,
                IsVegetarian = false,
                PreparationTime = 25,
                CreatedAt = DateTime.UtcNow
            };

            var caesarSalad = new MenuItem
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
                RestaurantId = restaurant.Id,
                CategoryId = saladCategory.Id,
                Name = "Caesar Salad",
                Description = "Fresh romaine lettuce with Caesar dressing",
                Price = 8.99m,
                IsAvailable = true,
                IsVegetarian = false,
                PreparationTime = 10,
                CreatedAt = DateTime.UtcNow
            };

            // Add to context
            await _context.Restaurants.AddAsync(restaurant);
            await _context.Categories.AddRangeAsync(pizzaCategory, saladCategory);
            await _context.MenuItems.AddRangeAsync(margheritaPizza, pepperoniPizza, caesarSalad);

            // Save changes
            await _context.SaveChangesAsync();
        }
    }
}
