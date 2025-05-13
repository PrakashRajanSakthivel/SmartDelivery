using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurentService.Application.Restaurents.Commands
{
    // RestaurantService.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantRequest.cs
    public record CreateRestaurantRequest(
        string Name,
        string Description,
        string? Address = null,
        string? PhoneNumber = null,
        decimal DeliveryFee = 0,
        decimal MinOrderAmount = 0);
}
