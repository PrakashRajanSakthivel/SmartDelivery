using MediatR;

namespace RestaurentService.Application.Restaurents.Commands
{
    public record DeleteRestaurantCommand(Guid RestaurantId) : IRequest<bool>;
} 