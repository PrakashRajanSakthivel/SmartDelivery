using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Application.Restaurents.Queries;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Entites;
using SharedSvc.Response;

namespace Restaurent.API
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController : BaseController
    {
        public RestaurantsController(IMediator mediator, ILogger<RestaurantsController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request)
        {
            var command = new CreateRestaurantCommand(request);
            var restaurantId = await _mediator.Send(command);
            return Created(restaurantId, "Restaurant created successfully");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(Guid id)
        {
            var query = new GetRestaurantById(id);
            var result = await _mediator.Send(query);
            return Success(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var query = new GetAllRestaurantsQuery();
            var result = await _mediator.Send(query);
            return Success(result);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetRestaurantDetails(Guid id)
        {
            var query = new GetRestaurantDetailsQuery(id);
            var result = await _mediator.Send(query);
            return Success(result);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRestaurantStatusRequest request)
        {
            if (id != request.RestaurantId)
                return BadRequest("RestaurantId mismatch");

            var command = new UpdateRestaurantStatusCommand(
                request.RestaurantId,
                request.NewStatus,
                request.Reason);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var query = new GetRestaurantStatus(id);
            var result = await _mediator.Send(query);
            return Success(result);
        }

    }
}