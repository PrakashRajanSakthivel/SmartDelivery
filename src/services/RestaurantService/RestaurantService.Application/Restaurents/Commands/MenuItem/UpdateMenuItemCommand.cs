using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.MenuItem
{
    public record UpdateMenuItemCommand(UpdateMenuItemRequest UpdateMenuItemRequest) : IRequest<bool>;
} 