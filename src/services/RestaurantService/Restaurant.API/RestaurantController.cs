using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Application.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Restaurent.API
{
    // RestaurantService.API/Controllers/RestaurantsController.cs
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RestaurantsController(IMediator mediator)
        => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request)
        {
            var command = new CreateRestaurantCommand(request);
            var restaurantId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurantId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(Guid id)
        {
            var query = new GetRestaurantById(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
