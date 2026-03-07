using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application;
using Microsoft.Extensions.Logging;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth")]

    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            _logger.LogInformation("Login attempt for user: {Username}", dto.Username);
            var result = await _mediator.Send(new LoginCommand(dto.Username, dto.Password));
            if (!result.Success)
            {
                _logger.LogWarning("Login failed for user: {Username}", dto.Username);
                return Unauthorized(new { message = result.Message });
            }
            // RESTful: 200 OK with token and user info
            return Ok(new
            {
                token = result.Token,
                user = result.User,
                message = result.Message,
                _links = new
                {
                    self = Url.Action(nameof(Login), "Auth")
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            _logger.LogInformation("Register attempt for user: {Username}", dto.Username);
            var result = await _mediator.Send(new RegisterCommand(dto.Username, dto.Password));
            if (!result.Success)
            {
                _logger.LogWarning("Register failed for user: {Username}", dto.Username);
                return BadRequest(new { message = result.Message });
            }
            // RESTful: 201 Created with token and user info
            return CreatedAtAction(nameof(Register), new
            {
                token = result.Token,
                user = result.User,
                message = result.Message,
                _links = new
                {
                    self = Url.Action(nameof(Register), "Auth")
                }
            });
        }
    }

    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
