using MediatR;
using RestaurentService.Application.Restaurents.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.Application.Restaurents.Queries
{
    public record GetRestaurantStatus(Guid Id) : IRequest<RestaurantStatusDto>;

}
