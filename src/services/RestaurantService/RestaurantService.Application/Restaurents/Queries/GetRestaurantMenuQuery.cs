using MediatR;

namespace RestaurentService.Application.Restaurents.Queries
{
    public record GetRestaurantMenuQuery(Guid RestaurantId) : IRequest<RestaurantMenuDto>;
} 