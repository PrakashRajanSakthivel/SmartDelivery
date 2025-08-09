using MediatR;
using PaymentService.Application.Payment.Commands;
using PaymentService.Application.Payment.DTO;
using PaymentService.Application.Contracts;
using PaymentService.Application.common;
using Microsoft.Extensions.Logging;

namespace PaymentService.Application.Payment.CommandHandlers
{
    public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, PaymentIntentResponse>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<CreatePaymentIntentCommandHandler> _logger;

        public CreatePaymentIntentCommandHandler(
            IPaymentService paymentService,
            ILogger<CreatePaymentIntentCommandHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<PaymentIntentResponse> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating payment intent for amount {Amount} {Currency}", 
                request.request.Amount, request.request.Currency);

            var response = await _paymentService.CreatePaymentIntentAsync(
                request.request.Amount, 
                request.request.Currency);

            _logger.LogInformation("Payment intent created with ID {PaymentIntentId}", response.Id);

            return response;
        }
    }
} 