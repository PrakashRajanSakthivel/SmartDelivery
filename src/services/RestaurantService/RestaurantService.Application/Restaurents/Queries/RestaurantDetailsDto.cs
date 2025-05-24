using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.Application.Restaurents.Queries
{
    public record RestaurantDetailsDto(
     Guid Id,
    string Name,
    string Description,
    decimal DeliveryFee,
    double AverageRating
     /* other fields */,
     List<CategoryDto> Categories,
     List<MenuItemDto> MenuItems);

    public record CategoryDto(
    string Name,
    int DisplayOrder);

    public record MenuItemDto(
        string Name,
        string? Description,
        decimal Price,
        Guid? CategoryId = null,  // Make nullable explicitly
        bool IsVegetarian = false,
        bool IsVegan = false,
        int PreparationTime = 15);
}
