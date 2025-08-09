using MediatR;
using PaymentService.Application.Payment.DTO;
using PaymentService.Application.Contracts;

namespace PaymentService.Application.Payment.Commands
{
    public record CreatePaymentIntentCommand(CreatePaymentIntentRequest request) : IRequest<PaymentIntentResponse>;
}
