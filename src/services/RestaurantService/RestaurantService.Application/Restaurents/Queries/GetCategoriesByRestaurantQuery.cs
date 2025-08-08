using MediatR;
using RestaurantService.Application.Restaurents.Queries;

namespace RestaurentService.Application.Restaurents.Queries
{
    public record GetCategoriesByRestaurantQuery(Guid RestaurantId) : IRequest<List<CategoryDto>>;
} 