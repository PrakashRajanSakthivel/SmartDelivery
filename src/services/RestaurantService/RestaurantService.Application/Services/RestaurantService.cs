using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Queries;

namespace RestaurentService.Application.Services
{
    // RestaurantService.Application/Services/RestaurantService.cs
    public class RestaurantService : IRestaurantService
    {
        private readonly IMediator _mediator;
        private readonly GetRestaurantbyId _getRestaurantQuery;

        public RestaurantService(IMediator mediator, GetRestaurantbyId getRestaurantQuery)
        {
            _mediator = mediator;
            _getRestaurantQuery = getRestaurantQuery;
        }

        // Command (uses MediatR)
        public async Task<Guid> CreateRestaurantAsync(CreateRestaurantRequest request)
        {
            var command = new CreateRestaurantCommand(
                request.Name,
                request.Description,
                request.Address,
                request.PhoneNumber,
                request.DeliveryFee,
                request.MinOrderAmount);

            return await _mediator.Send(command);
        }

        // Query (direct execution)
        public async Task<RestaurantDto> GetRestaurantAsync(Guid id)
        {
            return await _getRestaurantQuery.ExecuteAsync(id);
        }
    }
}
