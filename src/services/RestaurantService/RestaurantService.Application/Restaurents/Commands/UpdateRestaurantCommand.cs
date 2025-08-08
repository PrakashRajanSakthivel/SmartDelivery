using MediatR;
using RestaurentService.Application.Restaurents.Queries;

namespace RestaurentService.Application.Restaurents.Commands
{
    public record UpdateRestaurantCommand(UpdateRestaurantRequest UpdateRestaurantRequest) : IRequest<bool>;
}