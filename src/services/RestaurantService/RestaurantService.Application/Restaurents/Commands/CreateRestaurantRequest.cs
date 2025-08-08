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
        string? Address,
        string? PhoneNumber,
        decimal DeliveryFee,
        decimal MinOrderAmount,
        // Add these new collections
        List<CategoryRequest> Categories,
        List<MenuItemRequest> MenuItems);

    public record CategoryRequest(
        string Name,
        int DisplayOrder);

    public record MenuItemRequest(
        string Name,
        string? Description,
        decimal Price,
        int? CategoryIndex = null,  // Index into the categories array
        bool IsVegetarian = false,
        bool IsVegan = false,
        int PreparationTime = 15);
}

