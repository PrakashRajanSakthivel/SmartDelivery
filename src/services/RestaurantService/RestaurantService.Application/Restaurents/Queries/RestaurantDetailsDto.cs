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
        string? Description,
        decimal DeliveryFee,
        double AverageRating,
        int EstimatedDeliveryTime,
        string? CoverImageUrl,
        string? LogoUrl,
        List<CategoryDto> Categories,
        List<MenuItemDto> MenuItems);

    public record CategoryDto(
        Guid Id,
        string Name,
        int DisplayOrder,
        List<MenuItemDto> MenuItems);

    public record MenuItemDto(
        Guid Id,
        string Name,
        string? Description,
        decimal Price,
        bool IsAvailable = true,
        Guid? CategoryId = null,
        bool IsVegetarian = false,
        bool IsVegan = false,
        string? ImageUrl = null,
        int PreparationTime = 15);
}
