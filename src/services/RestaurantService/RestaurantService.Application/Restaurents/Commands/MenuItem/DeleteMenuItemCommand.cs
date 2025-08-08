using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.MenuItem
{
    public record DeleteMenuItemCommand(Guid MenuItemId, Guid RestaurantId) : IRequest<bool>;
} 