using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payment.Commands;
using PaymentService.Application.Payment.DTO;

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
            var command = new CreatePaymentIntentCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentRequest request)
        {
            var command = new ConfirmPaymentCommand(request);
            var result = await _mediator.Send(command);
            return Ok();
        }
    }


}
