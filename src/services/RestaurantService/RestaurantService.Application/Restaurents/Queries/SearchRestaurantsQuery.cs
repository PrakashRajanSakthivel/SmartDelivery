using MediatR;

namespace RestaurentService.Application.Restaurents.Queries
{
    public record SearchRestaurantsQuery(string SearchTerm) : IRequest<List<RestaurantDto>>;
} 