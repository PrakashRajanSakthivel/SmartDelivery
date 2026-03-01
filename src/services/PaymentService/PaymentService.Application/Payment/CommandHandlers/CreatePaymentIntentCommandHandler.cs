using MediatR;
using PaymentService.Application.common;
using PaymentService.Application.Contracts;
using PaymentService.Application.Payment.Commands;

namespace PaymentService.Application.Payment.CommandHandlers
{
    public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, PaymentIntentResponse>
    {
        private readonly IPaymentService _paymentService;

        public CreatePaymentIntentCommandHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public Task<PaymentIntentResponse> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
            => _paymentService.CreatePaymentIntentAsync(request.Request.Amount, request.Request.Currency);
    }
}
