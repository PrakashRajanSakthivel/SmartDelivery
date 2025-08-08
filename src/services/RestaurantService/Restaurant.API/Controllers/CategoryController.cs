//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using RestaurentService.Application.Restaurents.Commands.Category;
//using RestaurentService.Application.Restaurents.Queries;
//using SharedSvc.Response;

//namespace Restaurent.API.Controllers
//{
//    [ApiController]
//    [Route("api/categories")]
//    public class CategoryController : BaseController
//    {
//        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
//            : base(mediator, logger)
//        {
//        }

//        // Category CRUD Operations
//        [HttpPost]
//        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
//        {
//            var command = new CreateCategoryCommand(request);
//            var categoryId = await _mediator.Send(command);
//            return Created(categoryId, "Category created successfully");
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetCategory(Guid id)
//        {
//            var query = new GetCategoryByIdQuery(id);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpGet("restaurant/{restaurantId}")]
//        public async Task<IActionResult> GetCategoriesByRestaurant(Guid restaurantId)
//        {
//            var query = new GetCategoriesByRestaurantQuery(restaurantId);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
//        {
//            if (id != request.CategoryId)
//                return BadRequest("CategoryId mismatch");

//            var command = new UpdateCategoryCommand(request);
//            var result = await _mediator.Send(command);
//            return Success(result, "Category updated successfully");
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCategory(Guid id, [FromQuery] Guid restaurantId)
//        {
//            var command = new DeleteCategoryCommand(id, restaurantId);
//            await _mediator.Send(command);
//            return NoContent();
//        }

//        // Category with Menu Items
//        [HttpGet("{id}/menu-items")]
//        public async Task<IActionResult> GetCategoryWithMenuItems(Guid id)
//        {
//            var query = new GetCategoryWithMenuItemsQuery(id);
//            var result = await _mediator.Send(query);
//            return Success(result);
//        }

//        // Reorder Categories
//        [HttpPut("restaurant/{restaurantId}/reorder")]
//        public async Task<IActionResult> ReorderCategories(Guid restaurantId, [FromBody] ReorderCategoriesRequest request)
//        {
//            if (restaurantId != request.RestaurantId)
//                return BadRequest("RestaurantId mismatch");

//            var command = new ReorderCategoriesCommand(request);
//            await _mediator.Send(command);
//            return NoContent();
//        }
//    }
//} 