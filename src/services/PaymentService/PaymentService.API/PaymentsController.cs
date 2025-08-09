using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payment.Commands;
using PaymentService.Application.Payment.DTO;
using PaymentService.Application.Contracts;
using SharedSvc.Response;

namespace PaymentService.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : BaseController
    {
        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
            : base(mediator, logger)
        {
        }

        [HttpPost("intents")]
        public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentIntentRequest request)
        {
            var command = new CreatePaymentIntentCommand(request);
            var result = await _mediator.Send(command);
            return Success(result, "Payment intent created successfully");
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentRequest request)
        {
            var command = new ConfirmPaymentCommand(request);
            var result = await _mediator.Send(command);
            
            if (result.Succeeded)
            {
                return Success(result, "Payment confirmed successfully");
            }
            else
            {
                return BadRequest("Payment confirmation failed");
            }
        }
    }
}
