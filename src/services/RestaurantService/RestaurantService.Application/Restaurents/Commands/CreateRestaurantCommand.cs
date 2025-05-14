using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace RestaurentService.Application.Restaurents.Commands
{
    // RestaurantService.Application/Restaurants/Commands/CreateRestaurant/CreateRestaurantCommand.cs
    public record CreateRestaurantCommand(
        string Name,
        string Description,
        string Address,
        string PhoneNumber,
        decimal DeliveryFee,
        decimal MinOrderAmount) : IRequest<Guid>;
}
