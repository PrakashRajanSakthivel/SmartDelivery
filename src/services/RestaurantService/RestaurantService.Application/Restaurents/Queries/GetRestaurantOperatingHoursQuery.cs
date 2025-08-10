using MediatR;

namespace RestaurantService.Application.Restaurents.Queries
{
    public record GetRestaurantOperatingHoursQuery(Guid RestaurantId) : IRequest<RestaurantOperatingHoursDto>;
}
