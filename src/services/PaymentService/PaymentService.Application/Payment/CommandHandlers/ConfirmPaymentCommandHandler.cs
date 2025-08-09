using MediatR;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Payment.Commands;
using PaymentService.Application.Payment.DTO;
using PaymentService.Application.Contracts;
using PaymentService.Application.common;

namespace PaymentService.Application.Payment.CommandHandlers
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, PaymentResult>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<ConfirmPaymentCommandHandler> _logger;

        public ConfirmPaymentCommandHandler(
            IPaymentService paymentService,
            ILogger<ConfirmPaymentCommandHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<PaymentResult> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Confirming payment for intent {PaymentIntentId}", 
                request.request.PaymentIntentId);

            var result = await _paymentService.ConfirmPaymentAsync(request.request.PaymentIntentId);

            if (result.Succeeded)
            {
                _logger.LogInformation("Payment confirmed successfully for intent {PaymentIntentId}", 
                    request.request.PaymentIntentId);
            }
            else
            {
                _logger.LogWarning("Payment confirmation failed for intent {PaymentIntentId}: {Error}", 
                    request.request.PaymentIntentId, result.Error);
            }

            return result;
        }
    }
} 