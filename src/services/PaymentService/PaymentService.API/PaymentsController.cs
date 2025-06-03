using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.API
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost("intents")]
        public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentIntentRequest request)
        {
            return Ok();
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentRequest request)
        {
            return Ok();
        }
    }

    // Request DTOs
    public record CreatePaymentIntentRequest(decimal Amount, string Currency = "usd");
    public record ConfirmPaymentRequest(string PaymentIntentId);
}
