using PaymentService.Application.common;
using PaymentService.Application.Contracts;
using PaymentService.Domain;

namespace PaymentService.Infra
{
    public class DbPaymentService : IPaymentService
    {
        private readonly IPaymentUnitOfWork _uow;

        public DbPaymentService(IPaymentUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PaymentIntentResponse> CreatePaymentIntentAsync(decimal amount, string currency)
        {
            var record = new PaymentIntentRecord
            {
                Id = "pi_mock_" + Guid.NewGuid(),
                ClientSecret = "mock_client_secret_" + Random.Shared.Next(1000, 9999),
                Amount = (long)(amount * 100),
                Currency = currency,
                Status = "requires_payment_method",
                CreatedAt = DateTime.UtcNow
            };

            await _uow.PaymentIntents.AddAsync(record);
            await _uow.CommitAsync();

            return new PaymentIntentResponse(
                record.Id,
                record.ClientSecret,
                record.Amount,
                record.Currency,
                record.Status);
        }

        public async Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId)
        {
            var record = await _uow.PaymentIntents.GetByIdAsync(paymentIntentId);
            if (record is null)
                return new PaymentResult(false, "Payment intent not found");

            bool success = Random.Shared.Next(0, 10) > 1;
            record.Status = success ? "succeeded" : "failed";
            await _uow.CommitAsync();

            return new PaymentResult(success, success ? null : "Mock payment declined");
        }
    }
}
