//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using RestaurentService.Application.Restaurents.Queries;
//using SharedSvc.Response;

//namespace Restaurent.API.Controllers
//{
//    [ApiController]
//    [Route("api/restaurants/{restaurantId}/menu")]
//    public class RestaurantMenuController : BaseController
//    {
//        public RestaurantMenuController(IMediator mediator, ILogger<RestaurantMenuController> logger)
//            : base(mediator, logger)
//        {
//        }

//        // Customer-facing full menu
//        [HttpGet]
//        public async Task<IActionResult> GetRestaurantMenu(Guid restaurantId)
//        {
//            var query = new GetRestaurantMenuQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        // Customer-facing available items only
//        [HttpGet("available")]
//        public async Task<IActionResult> GetAvailableMenu(Guid restaurantId)
//        {
//            var query = new GetAvailableRestaurantMenuQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        // Menu by category for customers
//        [HttpGet("categories")]
//        public async Task<IActionResult> GetMenuByCategories(Guid restaurantId)
//        {
//            var query = new GetMenuByCategoriesQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }
//    }
//} 