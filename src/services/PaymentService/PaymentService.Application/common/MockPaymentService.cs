using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Contracts;

namespace PaymentService.Application.common
{
    public class MockPaymentService : IPaymentService
    {
        private readonly Dictionary<string, PaymentIntent> _mockIntents = new();

        public Task<PaymentIntentResponse> CreatePaymentIntentAsync(decimal amount, string currency)
        {
            var intent = new PaymentIntent
            {
                Id = "pi_mock_" + Guid.NewGuid(),
                ClientSecret = "mock_client_secret_" + Random.Shared.Next(1000, 9999),
                Amount = (long)(amount * 100), // Convert to cents
                Currency = currency,
                Status = "requires_payment_method"
            };

            _mockIntents[intent.Id] = intent;

            return Task.FromResult(new PaymentIntentResponse(
                intent.Id,
                intent.ClientSecret,
                intent.Amount,
                intent.Currency,
                intent.Status));
        }

        public Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId)
        {
            if (_mockIntents.TryGetValue(paymentIntentId, out var intent))
            {
                // Simulate 10% failure rate
                bool success = Random.Shared.Next(0, 10) > 1;

                intent.Status = success ? "succeeded" : "failed";

                return Task.FromResult(new PaymentResult(
                    success,
                    success ? null : "Mock payment declined"));
            }

            return Task.FromResult(new PaymentResult(false, "Payment intent not found"));
        }

       
    }
}
