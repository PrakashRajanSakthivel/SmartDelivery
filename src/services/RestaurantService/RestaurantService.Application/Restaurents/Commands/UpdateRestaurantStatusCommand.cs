using MediatR;
using RestaurentService.Domain.Entites;

namespace RestaurentService.Application.Restaurents.Commands
{
    public sealed record UpdateRestaurantStatusCommand(
        Guid RestaurantId,
        RestaurantStatus NewStatus,
        string? Reason = null) : IRequest<bool>;
}