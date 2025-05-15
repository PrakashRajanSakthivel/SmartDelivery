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
    //public class GetRestaurantbyId
    //{
    //    private readonly IRestaurantRepository _repository;

    //    public GetRestaurantbyId(IRestaurantRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    public async Task<RestaurantDto> ExecuteAsync(Guid id)
    //    {
    //        var restaurant = await _repository.GetByIdAsync(id);

    //        if (restaurant == null)
    //        {
    //            throw new KeyNotFoundException($"Restaurant with ID {id} not found");
    //        }

    //        return new RestaurantDto(
    //            restaurant.Id,
    //            restaurant.Name,
    //            restaurant.Description,
    //            restaurant.Address,
    //            restaurant.PhoneNumber,
    //            restaurant.DeliveryFee,
    //            restaurant.MinOrderAmount,
    //            restaurant.IsActive,
    //            restaurant.AverageRating);
    //    }
    //}
}
