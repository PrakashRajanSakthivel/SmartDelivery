using MediatR;

namespace RestaurentService.Application.Restaurents.Queries
{
    public record GetMenuItemsByRestaurantQuery(Guid RestaurantId) : IRequest<List<MenuItemDto>>;
} 