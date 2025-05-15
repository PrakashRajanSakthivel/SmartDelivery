using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurentService.Application.Restaurents.Queries
{
    // RestaurantService.Application/Restaurants/Queries/GetRestaurant/RestaurantDto.cs
    public record RestaurantDto(
    Guid Id,
    string Name,
    string Description,
    string? Address = null,  // Optional
    string? PhoneNumber = null,
    decimal DeliveryFee = 0,
    decimal MinOrderAmount = 0,
    bool IsActive = true,
    double AverageRating = 0
);
}
