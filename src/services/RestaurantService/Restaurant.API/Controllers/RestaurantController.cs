using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Application.Restaurents.Queries;
using RestaurentService.Application.Restaurents.Commands;
using RestaurentService.Application.Restaurents.Queries;
using RestaurentService.Domain.Entites;
using SharedSvc.Response;

namespace Restaurent.API.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantController : BaseController
    {
        public RestaurantController(IMediator mediator, ILogger<RestaurantController> logger)
            : base(mediator, logger)
        {
        }

        /// <summary>
        /// Create a new restaurant
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request)
        {
            _logger.LogInformation("Creating restaurant: {RestaurantName}", request.Name);
            
            var command = new CreateRestaurantCommand(request);
            var restaurantId = await _mediator.Send(command);
            
            return Created(restaurantId, "Restaurant created successfully");
        }

        /// <summary>
        /// Get restaurant by ID (basic info)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(Guid id)
        {
            _logger.LogInformation("Fetching restaurant: {RestaurantId}", id);
            
            var query = new GetRestaurantById(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Get all restaurants (admin use)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            _logger.LogInformation("Fetching all restaurants");
            
            var query = new GetAllRestaurantsQuery();
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Get active restaurants (customer-facing)
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRestaurants()
        {
            _logger.LogInformation("Fetching active restaurants");
            
            var query = new GetActiveRestaurantsQuery();
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Update restaurant details
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(Guid id, [FromBody] UpdateRestaurantRequest request)
        {
            if (id != request.RestaurantId)
            {
                _logger.LogWarning("RestaurantId mismatch: URL={UrlId}, Body={BodyId}", id, request.RestaurantId);
                return BadRequest("RestaurantId mismatch");
            }

            _logger.LogInformation("Updating restaurant: {RestaurantId}", id);
            
            var command = new UpdateRestaurantCommand(request);
            var result = await _mediator.Send(command);
            
            return Success(result, "Restaurant updated successfully");
        }

        /// <summary>
        /// Delete restaurant
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(Guid id)
        {
            _logger.LogInformation("Deleting restaurant: {RestaurantId}", id);
            
            var command = new DeleteRestaurantCommand(id);
            await _mediator.Send(command);
            
            return NoContent();
        }

        /// <summary>
        /// Get restaurant details with additional information
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetRestaurantDetails(Guid id)
        {
            _logger.LogInformation("Fetching restaurant details: {RestaurantId}", id);
            
            var query = new GetRestaurantDetailsQuery(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Get restaurant menu items (individual endpoint)
        /// </summary>
        [HttpGet("{id}/menu")]
        public async Task<IActionResult> GetRestaurantMenu(Guid id)
        {
            _logger.LogInformation("Fetching restaurant menu: {RestaurantId}", id);
            
            var query = new GetRestaurantMenuQuery(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Get restaurant operating hours (individual endpoint)
        /// </summary>
        [HttpGet("{id}/operating-hours")]
        public async Task<IActionResult> GetOperatingHours(Guid id)
        {
            _logger.LogInformation("Fetching restaurant operating hours: {RestaurantId}", id);
            
            var query = new GetRestaurantOperatingHoursQuery(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Check if restaurant is active for orders (individual endpoint)
        /// </summary>
        [HttpGet("{id}/is-active")]
        public async Task<IActionResult> IsRestaurantActive(Guid id)
        {
            _logger.LogInformation("Checking if restaurant is active: {RestaurantId}", id);
            
            var query = new IsRestaurantActiveQuery(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Get restaurant validation data (consolidated endpoint for external services)
        /// </summary>
        [HttpGet("{id}/validation")]
        public async Task<IActionResult> GetRestaurantForValidation(Guid id)
        {
            _logger.LogInformation("Fetching restaurant validation data: {RestaurantId}", id);
            
            var query = new GetRestaurantForValidationQuery(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Search restaurants by name, description, or menu items
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchRestaurants([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Search term cannot be empty");
            }

            _logger.LogInformation("Searching restaurants with term: {SearchTerm}", term);
            
            var query = new SearchRestaurantsQuery(term);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }

        /// <summary>
        /// Update restaurant status (Active, Inactive, etc.)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRestaurantStatusRequest request)
        {
            if (id != request.RestaurantId)
            {
                _logger.LogWarning("RestaurantId mismatch in status update: URL={UrlId}, Body={BodyId}", id, request.RestaurantId);
                return BadRequest("RestaurantId mismatch");
            }

            _logger.LogInformation("Updating restaurant status: {RestaurantId} to {Status}", id, request.NewStatus);
            
            var command = new UpdateRestaurantStatusCommand(
                request.RestaurantId,
                request.NewStatus,
                request.Reason);

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Get restaurant status
        /// </summary>
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            _logger.LogInformation("Fetching restaurant status: {RestaurantId}", id);
            
            var query = new GetRestaurantStatus(id);
            var result = await _mediator.Send(query);
            
            return Success(result);
        }
    }
} 