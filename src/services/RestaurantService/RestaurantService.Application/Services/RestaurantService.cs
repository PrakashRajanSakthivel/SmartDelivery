using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Interfaces;

namespace RestaurentService.Application.Services
{
    // RestaurantService.Application/Services/RestaurantService.cs
    public class RestaurantService : IRestaurantService
    {
        //private readonly IMediator _mediator;
        //private readonly IRestaurantRepository _repository;

        //public RestaurantService(IMediator mediator, IRestaurantRepository repository)
        //{
        //    _mediator = mediator;
        //    _repository = repository;
        //}

        //// Command (uses MediatR)
        //public async Task<Guid> CreateRestaurantAsync(CreateRestaurantRequest request)
        //{
        //    var command = new CreateRestaurantCommand(
        //        request.Name,
        //        request.Description,
        //        request.Address,
        //        request.PhoneNumber,
        //        request.DeliveryFee,
        //        request.MinOrderAmount);

        //    return await _mediator.Send(command);
        //}

        //// Query (direct execution)
        //public async Task<RestaurantDto> GetRestaurantAsync(Guid id)
        //{
        //    var restaurant = await _repository.GetByIdAsync(id);

        //    if (restaurant == null)
        //        return null; // Or throw NotFoundException

        //    return new RestaurantDto(
        //        restaurant.Id,
        //        restaurant.Name,
        //        restaurant.Description,
        //        restaurant.Address,
        //        restaurant.PhoneNumber,
        //        restaurant.DeliveryFee,
        //        restaurant.MinOrderAmount,
        //        restaurant.IsActive,
        //        restaurant.AverageRating
        //     );

        //}
    }
}
