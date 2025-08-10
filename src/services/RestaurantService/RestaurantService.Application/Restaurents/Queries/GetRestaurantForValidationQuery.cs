using MediatR;

namespace RestaurantService.Application.Restaurents.Queries
{
    public record GetRestaurantForValidationQuery(Guid RestaurantId) : IRequest<RestaurantValidationDto>;
}
