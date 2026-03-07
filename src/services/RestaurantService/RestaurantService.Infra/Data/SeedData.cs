using RestaurentService.Domain.Entites;
using RestaurentService.Infra.Data;

namespace RestaurantService.Infra.Data
{
    public static class SeedData
    {
        public static void Initialize(RestaurantDbContext context)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                return;

            if (context.Restaurants.Any())
                return;

            // --- Burger House ---
            var burgerHouse = new Restaurant
            {
                Id = Guid.Parse("11111111-0000-0000-0000-000000000001"),
                Name = "Burger House",
                Description = "Juicy burgers and crispy fries",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                AverageRating = 4.3,
                DeliveryFee = 1.99m,
                MinOrderAmount = 8.00m,
                PhoneNumber = "555-100-0001",
                EstimatedDeliveryTime = 20,
                CoverImageUrl = null,
                LogoUrl = null,
                Address = "10 Burger Lane"
            };

            var burgersCat = new Category { Id = Guid.Parse("22221111-0000-0000-0000-000000000001"), RestaurantId = burgerHouse.Id, Name = "Burgers", DisplayOrder = 1 };
            var sidesCat   = new Category { Id = Guid.Parse("22221111-0000-0000-0000-000000000002"), RestaurantId = burgerHouse.Id, Name = "Sides",   DisplayOrder = 2 };
            var drinksCat  = new Category { Id = Guid.Parse("22221111-0000-0000-0000-000000000003"), RestaurantId = burgerHouse.Id, Name = "Drinks",  DisplayOrder = 3 };

            var burgerItems = new List<MenuItem>
            {
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = burgersCat.Id, Name = "Classic Cheeseburger", Description = "Beef patty, cheese, lettuce, tomato, onion", Price = 9.99m, IsAvailable = true, IsVegetarian = false, CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = burgersCat.Id, Name = "Veggie Burger",         Description = "Plant-based patty, cheese, lettuce, tomato",     Price = 10.99m, IsAvailable = true, IsVegetarian = true,  CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = burgersCat.Id, Name = "Bacon Double",          Description = "Double beef, bacon, cheddar, pickles",            Price = 12.99m, IsAvailable = true, IsVegetarian = false, CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = sidesCat.Id,   Name = "Crispy Fries",          Description = "Golden salted fries",                             Price = 3.49m,  IsAvailable = true, IsVegetarian = true,  CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = sidesCat.Id,   Name = "Onion Rings",           Description = "Beer-battered onion rings",                       Price = 4.49m,  IsAvailable = true, IsVegetarian = true,  CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = drinksCat.Id,  Name = "Cola",                  Description = "Chilled Coca-Cola 500 ml",                        Price = 2.49m,  IsAvailable = true, IsVegetarian = true,  CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = burgerHouse.Id, CategoryId = drinksCat.Id,  Name = "Milkshake",             Description = "Vanilla, chocolate or strawberry",                Price = 4.99m,  IsAvailable = true, IsVegetarian = true,  CreatedAt = DateTime.UtcNow },
            };

            // --- Pizza Palace ---
            var pizzaPalace = new Restaurant
            {
                Id = Guid.Parse("11111111-0000-0000-0000-000000000002"),
                Name = "Pizza Palace",
                Description = "Wood-fired pizzas made fresh daily",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                AverageRating = 4.6,
                DeliveryFee = 2.49m,
                MinOrderAmount = 12.00m,
                PhoneNumber = "555-100-0002",
                EstimatedDeliveryTime = 30,
                Address = "22 Pizza Street"
            };

            var pizzasCat  = new Category { Id = Guid.Parse("22222222-0000-0000-0000-000000000001"), RestaurantId = pizzaPalace.Id, Name = "Pizzas",  DisplayOrder = 1 };
            var pastasCat  = new Category { Id = Guid.Parse("22222222-0000-0000-0000-000000000002"), RestaurantId = pizzaPalace.Id, Name = "Pastas",  DisplayOrder = 2 };

            var pizzaItems = new List<MenuItem>
            {
                new() { Id = Guid.NewGuid(), RestaurantId = pizzaPalace.Id, CategoryId = pizzasCat.Id, Name = "Margherita",      Description = "Tomato, mozzarella, fresh basil",          Price = 11.99m, IsAvailable = true, IsVegetarian = true,  CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = pizzaPalace.Id, CategoryId = pizzasCat.Id, Name = "Pepperoni",       Description = "Tomato, mozzarella, pepperoni",            Price = 13.99m, IsAvailable = true, IsVegetarian = false, CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = pizzaPalace.Id, CategoryId = pizzasCat.Id, Name = "BBQ Chicken",     Description = "BBQ base, chicken, red onion, coriander",  Price = 14.99m, IsAvailable = true, IsVegetarian = false, CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = pizzaPalace.Id, CategoryId = pastasCat.Id, Name = "Spaghetti Bolognese", Description = "Slow-cooked beef ragù, parmesan",      Price = 12.49m, IsAvailable = true, IsVegetarian = false, CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), RestaurantId = pizzaPalace.Id, CategoryId = pastasCat.Id, Name = "Penne Arrabbiata",    Description = "Spicy tomato, garlic, basil",          Price = 10.99m, IsAvailable = true, IsVegetarian = true,  IsVegan = true, CreatedAt = DateTime.UtcNow },
            };

            context.Restaurants.AddRange(burgerHouse, pizzaPalace);
            context.Categories.AddRange(burgersCat, sidesCat, drinksCat, pizzasCat, pastasCat);
            context.MenuItems.AddRange(burgerItems);
            context.MenuItems.AddRange(pizzaItems);
            context.SaveChanges();
        }
    }
}
