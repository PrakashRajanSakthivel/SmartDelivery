using PaymentService.Domain.Entites;

namespace PaymentService.Application.Payments.Queries
{
    public class PaymentDto
    {
        public Guid PaymentId { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public Guid? OrderId { get; set; }
        public Guid? UserId { get; set; }
        
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public PaymentStatus Status { get; set; }
        
        public string? ClientSecret { get; set; }
        public string? PaymentMethodId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        
        public string? ProviderTransactionId { get; set; }
        public string? Description { get; set; }
        
        public int RetryCount { get; set; }
        public DateTime? LastRetryAt { get; set; }
        
    }
}