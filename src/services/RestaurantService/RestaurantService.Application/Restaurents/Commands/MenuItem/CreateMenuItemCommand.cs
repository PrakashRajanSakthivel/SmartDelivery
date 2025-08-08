using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.MenuItem
{
    public record CreateMenuItemCommand(CreateMenuItemRequest CreateMenuItemRequest) : IRequest<Guid>;
} 