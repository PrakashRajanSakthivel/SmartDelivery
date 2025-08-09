using Microsoft.Extensions.Logging;

namespace PaymentService.Application.Services
{
    public class SimplePaymentService : IExternalPaymentService
    {
        private readonly ILogger<SimplePaymentService> _logger;

        public SimplePaymentService(ILogger<SimplePaymentService> logger)
        {
            _logger = logger;
        }

        public Task<ExternalPaymentIntentResponse> CreatePaymentIntentAsync(
            decimal amount, 
            string currency, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating simple mock payment intent for {Amount} {Currency}", amount, currency);

            var response = new ExternalPaymentIntentResponse
            {
                Id = "pi_simple_" + Guid.NewGuid().ToString("N")[..12],
                ClientSecret = "secret_" + Guid.NewGuid().ToString("N")[..8],
                Amount = (long)amount,
                Currency = currency.ToLower(),
                Status = "requires_payment_method"
            };

            _logger.LogInformation("Created payment intent: {PaymentIntentId}", response.Id);
            return Task.FromResult(response);
        }

        public Task<ExternalPaymentConfirmationResponse> ConfirmPaymentAsync(
            string paymentIntentId, 
            string? paymentMethodId = null, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Confirming payment intent {PaymentIntentId}", paymentIntentId);

            // Simple 90% success rate
            var isSuccess = Random.Shared.Next(0, 100) < 90;

            var response = new ExternalPaymentConfirmationResponse
            {
                Status = isSuccess ? "succeeded" : "failed",
                Error = isSuccess ? null : "card_declined",
                PaymentIntent = new PaymentIntentData
                {
                    Id = paymentIntentId,
                    Status = isSuccess ? "succeeded" : "requires_payment_method"
                }
            };

            _logger.LogInformation("Payment {PaymentIntentId} result: {Status}", paymentIntentId, response.Status);
            return Task.FromResult(response);
        }
    }
}