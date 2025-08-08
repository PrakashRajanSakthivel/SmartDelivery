using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RestaurentService.Application.Restaurents.Queries;

namespace RestaurantService.Application.Restaurents.Queries
{
        public record GetAllRestaurantsQuery : IRequest<List<RestaurantDto>>;
}
