using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Services;

namespace Restaurent.API
{
    // RestaurantService.API/Controllers/RestaurantsController.cs
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request)
        {
            var restaurantId = await _restaurantService.CreateRestaurantAsync(request);
            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurantId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(Guid id)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            return Ok(restaurant);
        }
    }
}
