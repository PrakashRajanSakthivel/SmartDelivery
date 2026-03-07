using MediatR;
using PaymentService.Application.Contracts;
using PaymentService.Application.Payment.DTO;

namespace PaymentService.Application.Payment.Commands
{
    public record CreatePaymentIntentCommand(CreatePaymentIntentRequest Request) : IRequest<PaymentIntentResponse>;
}
