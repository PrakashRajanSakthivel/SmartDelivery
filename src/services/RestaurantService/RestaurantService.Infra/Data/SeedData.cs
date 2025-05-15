using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurentService.Domain.Entites;
using RestaurentService.Infra.Data;

namespace RestaurantService.Infra.Data
{
    public static class SeedData
    {
        public static void Initialize(RestaurantDbContext context)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                if (!context.Restaurants.Any())
                {
                    context.Restaurants.AddRange(
                        new Restaurant { 
                        Id = Guid.NewGuid(),
                        Name = "Burger Palace",
                        Description = "Home of the best burgers in town",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        AverageRating = 4.5,
                        DeliveryFee = 2.99m,
                        MinOrderAmount = 10.00m,
                        PhoneNumber = "123-456-7890",
                        EstimatedDeliveryTime = 30
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
