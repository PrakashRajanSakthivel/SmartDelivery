//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using RestaurentService.Application.Restaurents.Commands.MenuItem;
//using RestaurentService.Application.Restaurents.Queries;
//using SharedSvc.Response;

//namespace Restaurent.API.Controllers
//{
//    [ApiController]
//    [Route("api/menu-items")]
//    public class MenuItemController : BaseController
//    {
//        public MenuItemController(IMediator mediator, ILogger<MenuItemController> logger)
//            : base(mediator, logger)
//        {
//        }

//        // Menu Item CRUD Operations
//        [HttpPost]
//        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemRequest request)
//        {
//            var command = new CreateMenuItemCommand(request);
//            var menuItemId = await _mediator.Send(command);
//            return Created(menuItemId, "Menu item created successfully");
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetMenuItem(Guid id)
//        {
//            //var query = new GetMenuItemByIdQuery(id);
//            //var result = await _mediator.Send(query);
//            //return Success(result);
//        }

//        [HttpGet("restaurant/{restaurantId}")]
//        public async Task<IActionResult> GetMenuItemsByRestaurant(Guid restaurantId)
//        {
//            var query = new GetMenuItemsByRestaurantQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateMenuItem(Guid id, [FromBody] UpdateMenuItemRequest request)
//        {
//            if (id != request.MenuItemId)
//                return BadRequest("MenuItemId mismatch");

//            var command = new UpdateMenuItemCommand(request);
//            var result = await _mediator.Send(command);
//            return Success(result, "Menu item updated successfully");
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteMenuItem(Guid id, [FromQuery] Guid restaurantId)
//        {
//            var command = new DeleteMenuItemCommand(id, restaurantId);
//            await _mediator.Send(command);
//            return NoContent();
//        }

//        // Menu Item Availability Management
//        [HttpPut("{id}/availability")]
//        public async Task<IActionResult> UpdateAvailability(Guid id, [FromBody] UpdateMenuItemAvailabilityRequest request)
//        {
//            if (id != request.MenuItemId)
//                return BadRequest("MenuItemId mismatch");

//            var command = new UpdateMenuItemAvailabilityCommand(request.MenuItemId, request.IsAvailable);
//            await _mediator.Send(command);
//            return NoContent();
//        }

//        // Menu Item Search & Filtering
//        [HttpGet("restaurant/{restaurantId}/available")]
//        public async Task<IActionResult> GetAvailableMenuItems(Guid restaurantId)
//        {
//            var query = new GetAvailableMenuItemsQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpGet("restaurant/{restaurantId}/category/{categoryId}")]
//        public async Task<IActionResult> GetMenuItemsByCategory(Guid restaurantId, Guid categoryId)
//        {
//            var query = new GetMenuItemsByCategoryQuery(restaurantId, categoryId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }
//    }
//} 