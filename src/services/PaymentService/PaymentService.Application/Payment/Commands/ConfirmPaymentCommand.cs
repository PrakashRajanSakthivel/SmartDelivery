using MediatR;
using PaymentService.Application.Contracts;
using PaymentService.Application.Payment.DTO;

namespace PaymentService.Application.Payment.Commands
{
    public record ConfirmPaymentCommand(ConfirmPaymentRequest Request) : IRequest<PaymentResult>;
}
