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
        string? Address,
        string? PhoneNumber,
        decimal DeliveryFee,
        decimal MinOrderAmount,
        bool IsActive,
        double AverageRating);
}
