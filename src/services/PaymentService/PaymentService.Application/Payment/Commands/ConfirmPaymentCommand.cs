using MediatR;
using PaymentService.Application.Payment.DTO;
using PaymentService.Application.Contracts;

namespace PaymentService.Application.Payment.Commands
{
    public record ConfirmPaymentCommand(ConfirmPaymentRequest request) : IRequest<PaymentResult>;
}
