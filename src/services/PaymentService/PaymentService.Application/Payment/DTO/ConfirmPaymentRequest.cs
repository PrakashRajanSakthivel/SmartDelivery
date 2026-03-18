using System.Text.Json.Serialization;

namespace PaymentService.Application.Payment.DTO
{
    public record ConfirmPaymentRequest([property: JsonPropertyName("paymentIntentId")] string PaymentIntentId);
}
