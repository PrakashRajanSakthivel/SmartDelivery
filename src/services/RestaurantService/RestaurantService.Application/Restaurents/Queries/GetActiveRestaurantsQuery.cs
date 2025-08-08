using MediatR;

namespace RestaurentService.Application.Restaurents.Queries
{
    public record GetActiveRestaurantsQuery() : IRequest<List<RestaurantDto>>;
} 