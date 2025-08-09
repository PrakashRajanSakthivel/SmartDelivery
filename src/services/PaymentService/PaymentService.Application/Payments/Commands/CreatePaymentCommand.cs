using MediatR;
using PaymentService.Application.Payments.DTO;
using PaymentService.Application.Payments.Queries;

namespace PaymentService.Application.Payments.Commands
{
    public record CreatePaymentCommand(CreatePaymentRequest Request) : IRequest<PaymentDto>;
}