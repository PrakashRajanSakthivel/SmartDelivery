using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurentService.Application.Restaurents.Queries
{
    public record RestaurantDto(
        Guid Id,
        string Name,
        string? Description,
        int EstimatedDeliveryTime = 0,
        string? CoverImageUrl = null,
        string? LogoUrl = null,
        string? Address = null,
        string? PhoneNumber = null,
        decimal DeliveryFee = 0,
        decimal MinOrderAmount = 0,
        bool IsActive = true,
        double AverageRating = 0
    );
}
