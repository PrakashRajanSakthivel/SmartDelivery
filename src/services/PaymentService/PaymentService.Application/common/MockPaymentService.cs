using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace PaymentService.Application.common
{
    public class MockPaymentService : IPaymentService
    {
        private readonly Dictionary<string, PaymentIntent> _mockIntents = new();
        private readonly ILogger<MockPaymentService> _logger;

        public MockPaymentService(ILogger<MockPaymentService> logger)
        {
            _logger = logger;
        }

        public async Task<PaymentIntentResponse> CreatePaymentIntentAsync(decimal amount, string currency)
        {
            // Simulate some processing delay
            await Task.Delay(100);

            var intent = new PaymentIntent
            {
                Id = "pi_mock_" + Guid.NewGuid().ToString("N")[..8],
                ClientSecret = "mock_client_secret_" + Random.Shared.Next(1000, 9999),
                Amount = (long)amount, // Store as provided (no cents conversion)
                Currency = currency,
                Status = "requires_payment_method"
            };

            _mockIntents[intent.Id] = intent;

            _logger.LogInformation("Mock payment intent created: {PaymentIntentId} for ${Amount}", 
                intent.Id, amount);

            return new PaymentIntentResponse(
                intent.Id,
                intent.ClientSecret,
                intent.Amount,
                intent.Currency,
                intent.Status);
        }

        public async Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId)
        {
            // Simulate processing delay
            await Task.Delay(200);

            if (_mockIntents.TryGetValue(paymentIntentId, out var intent))
            {
                // Simulate realistic scenarios:
                // 85% success, 10% insufficient funds, 5% card declined
                var outcome = Random.Shared.Next(0, 100);
                
                bool success;
                string? error = null;

                if (outcome < 85)
                {
                    success = true;
                    intent.Status = "succeeded";
                }
                else if (outcome < 95)
                {
                    success = false;
                    error = "Insufficient funds";
                    intent.Status = "failed";
                }
                else
                {
                    success = false;
                    error = "Card declined";
                    intent.Status = "failed";
                }

                _logger.LogInformation("Mock payment {Status}: {PaymentIntentId} - {Error}", 
                    success ? "succeeded" : "failed", paymentIntentId, error ?? "N/A");

                return new PaymentResult(success, error);
            }

            _logger.LogWarning("Payment intent not found: {PaymentIntentId}", paymentIntentId);
            return new PaymentResult(false, "Payment intent not found");
        }
    }
}
