namespace PaymentService.Application.Services
{
    public interface IExternalPaymentService
    {
        Task<ExternalPaymentIntentResponse> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken cancellationToken = default);
        Task<ExternalPaymentConfirmationResponse> ConfirmPaymentAsync(string paymentIntentId, string? paymentMethodId = null, CancellationToken cancellationToken = default);
    }

    // External provider response models
    public class ExternalPaymentIntentResponse
    {
        public string Id { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class ExternalPaymentConfirmationResponse
    {
        public string Status { get; set; } = string.Empty;
        public string? Error { get; set; }
        public PaymentIntentData? PaymentIntent { get; set; }
    }

    public class PaymentIntentData
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}