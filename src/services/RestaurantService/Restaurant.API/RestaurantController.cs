//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using RestaurantService.Application.Restaurents.Queries;
//using RestaurentService.Application.Restaurents.Commands;
//using RestaurentService.Application.Restaurents.Commands.MenuItem;
//using RestaurentService.Application.Restaurents.Commands.Category;
//using RestaurentService.Domain.Entites;
//using SharedSvc.Response;
//using RestaurentService.Application.Restaurents.Queries;

//namespace Restaurent.API
//{
//    [ApiController]
//    [Route("api/restaurants")]
//    public class RestaurantsController : BaseController
//    {
//        public RestaurantsController(IMediator mediator, ILogger<RestaurantsController> logger)
//            : base(mediator, logger)
//        {
//        }

//        // Restaurant CRUD
//        [HttpPost]
//        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantRequest request)
//        {
//            var command = new CreateRestaurantCommand(request);
//            var restaurantId = await _mediator.Send(command);
//            return Created(restaurantId, "Restaurant created successfully");
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetRestaurant(Guid id)
//        {
//            var query = new GetRestaurantById(id);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllRestaurants()
//        {
//            var query = new GetAllRestaurantsQuery();
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpGet("active")]
//        public async Task<IActionResult> GetActiveRestaurants()
//        {
//            var query = new GetActiveRestaurantsQuery();
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateRestaurant(Guid id, [FromBody] UpdateRestaurantRequest request)
//        {
//            if (id != request.RestaurantId)
//                return BadRequest("RestaurantId mismatch");

//            var command = new UpdateRestaurantCommand(request);
//            var result = await _mediator.Send(command);
//            return Success(result, "Restaurant updated successfully");
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteRestaurant(Guid id)
//        {
//            var command = new DeleteRestaurantCommand(id);
//            await _mediator.Send(command);
//            return NoContent();
//        }

//        [HttpGet("{id}/details")]
//        public async Task<IActionResult> GetRestaurantDetails(Guid id)
//        {
//            var query = new GetRestaurantDetailsQuery(id);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpGet("{id}/menu")]
//        public async Task<IActionResult> GetRestaurantMenu(Guid id)
//        {
//            var query = new GetRestaurantMenuQuery(id);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpGet("search")]
//        public async Task<IActionResult> SearchRestaurants([FromQuery] string term)
//        {
//            var query = new SearchRestaurantsQuery(term);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        // Restaurant Status
//        [HttpPut("{id}/status")]
//        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRestaurantStatusRequest request)
//        {
//            if (id != request.RestaurantId)
//                return BadRequest("RestaurantId mismatch");

//            var command = new UpdateRestaurantStatusCommand(
//                request.RestaurantId, 
//                request.NewStatus, 
//                request.Reason);

//            await _mediator.Send(command);
//            return NoContent();
//        }

//        [HttpGet("{id}/status")]
//        public async Task<IActionResult> GetStatus(Guid id)
//        {
//            var query = new GetRestaurantStatus(id);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        // Menu Items
//        [HttpPost("{restaurantId}/menu-items")]
//        public async Task<IActionResult> CreateMenuItem(Guid restaurantId, [FromBody] CreateMenuItemRequest request)
//        {
//            if (restaurantId != request.RestaurantId)
//                return BadRequest("RestaurantId mismatch");

//            var command = new CreateMenuItemCommand(request);
//            var menuItemId = await _mediator.Send(command);
//            return Created(menuItemId, "Menu item created successfully");
//        }

//        [HttpGet("{restaurantId}/menu-items")]
//        public async Task<IActionResult> GetMenuItems(Guid restaurantId)
//        {
//            var query = new GetMenuItemsByRestaurantQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpPut("menu-items/{menuItemId}")]
//        public async Task<IActionResult> UpdateMenuItem(Guid menuItemId, [FromBody] UpdateMenuItemRequest request)
//        {
//            if (menuItemId != request.MenuItemId)
//                return BadRequest("MenuItemId mismatch");

//            var command = new UpdateMenuItemCommand(request);
//            var result = await _mediator.Send(command);
//            return Success(result, "Menu item updated successfully");
//        }

//        [HttpDelete("menu-items/{menuItemId}")]
//        public async Task<IActionResult> DeleteMenuItem(Guid menuItemId, [FromQuery] Guid restaurantId)
//        {
//            var command = new DeleteMenuItemCommand(menuItemId, restaurantId);
//            await _mediator.Send(command);
//            return NoContent();
//        }

//        [HttpPut("menu-items/{menuItemId}/availability")]
//        public async Task<IActionResult> UpdateMenuItemAvailability(Guid menuItemId, [FromBody] UpdateMenuItemAvailabilityRequest request)
//        {
//            if (menuItemId != request.MenuItemId)
//                return BadRequest("MenuItemId mismatch");

//            var command = new UpdateMenuItemAvailabilityCommand(request.MenuItemId, request.IsAvailable);
//            await _mediator.Send(command);
//            return NoContent();
//        }

//        // Categories
//        [HttpPost("{restaurantId}/categories")]
//        public async Task<IActionResult> CreateCategory(Guid restaurantId, [FromBody] CreateCategoryRequest request)
//        {
//            if (restaurantId != request.RestaurantId)
//                return BadRequest("RestaurantId mismatch");

//            var command = new CreateCategoryCommand(request);
//            var categoryId = await _mediator.Send(command);
//            return Created(categoryId, "Category created successfully");
//        }

//        [HttpGet("{restaurantId}/categories")]
//        public async Task<IActionResult> GetCategories(Guid restaurantId)
//        {
//            var query = new GetCategoriesByRestaurantQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpPut("categories/{categoryId}")]
//        public async Task<IActionResult> UpdateCategory(Guid categoryId, [FromBody] UpdateCategoryRequest request)
//        {
//            if (categoryId != request.CategoryId)
//                return BadRequest("CategoryId mismatch");

//            var command = new UpdateCategoryCommand(request);
//            var result = await _mediator.Send(command);
//            return Success(result, "Category updated successfully");
//        }

//        [HttpDelete("categories/{categoryId}")]
//        public async Task<IActionResult> DeleteCategory(Guid categoryId, [FromQuery] Guid restaurantId)
//        {
//            var command = new DeleteCategoryCommand(categoryId, restaurantId);
//            await _mediator.Send(command);
//            return NoContent();
//        }
//    }
//}