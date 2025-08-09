using MediatR;
using PaymentService.Application.Payments.DTO;
using PaymentService.Application.Payments.Queries;

namespace PaymentService.Application.Payments.Commands
{
    public record ConfirmPaymentCommand(ConfirmPaymentRequest Request) : IRequest<PaymentDto>;
}