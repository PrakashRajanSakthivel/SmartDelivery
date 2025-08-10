using MediatR;

namespace RestaurantService.Application.Restaurents.Queries
{
    public record IsRestaurantActiveQuery(Guid RestaurantId) : IRequest<RestaurantActiveStatusDto>;
}
