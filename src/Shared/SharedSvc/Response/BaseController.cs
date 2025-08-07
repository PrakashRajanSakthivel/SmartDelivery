using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedSvc.Response;

namespace SharedSvc.Response
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;

        protected BaseController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected IActionResult Success<T>(T data, string? message = null)
        {
            var response = ApiResponse<T>.SuccessM(data, message);
            
            return Ok(response);
        }

        protected IActionResult Created<T>(T data, string? message = null)
        {
            var response = ApiResponse<T>.SuccessM(data, message);
            
            return StatusCode(StatusCodes.Status201Created, response);
        }

        protected IActionResult NoContent()
        {
            return StatusCode(StatusCodes.Status204NoContent);
        }

        protected IActionResult BadRequest(string message, List<ApiError>? errors = null)
        {
            var response = ApiResponse<object>.Error(message, errors);
            
            return StatusCode(StatusCodes.Status400BadRequest, response);
        }

        protected IActionResult NotFound(string message = "Resource not found")
        {
            var response = ApiResponse<object>.Error(message);
            
            return StatusCode(StatusCodes.Status404NotFound, response);
        }

        protected IActionResult Unauthorized(string message = "Unauthorized")
        {
            var response = ApiResponse<object>.Error(message);
            
            return StatusCode(StatusCodes.Status401Unauthorized, response);
        }

        protected IActionResult Forbidden(string message = "Forbidden")
        {
            var response = ApiResponse<object>.Error(message);
            
            return StatusCode(StatusCodes.Status403Forbidden, response);
        }

        protected IActionResult InternalServerError(string message = "An internal server error occurred")
        {
            var response = ApiResponse<object>.Error(message);
            
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        protected IActionResult ValidationError(List<ApiError> errors)
        {
            var response = ApiResponse<object>.ValidationError(errors);
            
            return BadRequest(response);
        }
    }
}