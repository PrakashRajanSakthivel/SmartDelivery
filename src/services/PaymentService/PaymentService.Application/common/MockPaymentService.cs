using PaymentService.Application.Contracts;

namespace PaymentService.Application.common
{
    public class MockPaymentService : IPaymentService
    {
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

            return Task.FromResult(new PaymentIntentResponse(
                intent.Id,
                intent.ClientSecret,
                intent.Amount,
                intent.Currency,
                intent.Status));
        }

        public Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId)
        {
            if (string.IsNullOrEmpty(paymentIntentId) || !paymentIntentId.StartsWith("pi_mock_"))
                return Task.FromResult(new PaymentResult(false, "Payment intent not found"));

            // Simulate 10% failure rate
            bool success = Random.Shared.Next(0, 10) > 1;

            return Task.FromResult(new PaymentResult(
                success,
                success ? null : "Mock payment declined"));
        }

       
    }
}
