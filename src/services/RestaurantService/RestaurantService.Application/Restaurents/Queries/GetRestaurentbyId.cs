using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RestaurentService.Domain.Interfaces;

namespace RestaurentService.Application.Restaurents.Queries
{
    // RestaurantService.Application/Restaurants/Queries/GetRestaurant/GetRestaurantQuery.cs

    public record GetRestaurantById(Guid Id) : IRequest<RestaurantDto>;

}
