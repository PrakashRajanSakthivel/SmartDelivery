using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Payments.Commands;
using PaymentService.Application.Payments.DTO;
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
        public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentRequest request)
        {
            var command = new CreatePaymentCommand(request);
            var result = await _mediator.Send(command);
            return Success(result, "Payment intent created successfully");
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentRequest request)
        {
            var command = new ConfirmPaymentCommand(request);
            var result = await _mediator.Send(command);
            
            if (result.Status == PaymentService.Domain.Entites.PaymentStatus.Succeeded)
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
