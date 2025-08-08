using MediatR;

namespace RestaurentService.Application.Restaurents.Commands.MenuItem
{
    public record UpdateMenuItemAvailabilityCommand(Guid MenuItemId, bool IsAvailable) : IRequest<bool>;
} 