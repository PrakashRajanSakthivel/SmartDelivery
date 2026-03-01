using MediatR;
using PaymentService.Application.common;
using PaymentService.Application.Contracts;
using PaymentService.Application.Payment.Commands;

namespace PaymentService.Application.Payment.CommandHandlers
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, PaymentResult>
    {
        private readonly IPaymentService _paymentService;

        public ConfirmPaymentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public Task<PaymentResult> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
            => _paymentService.ConfirmPaymentAsync(request.Request.PaymentIntentId);
    }
}
