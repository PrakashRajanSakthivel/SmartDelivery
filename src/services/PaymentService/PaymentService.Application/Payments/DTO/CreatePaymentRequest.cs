using System;

namespace PaymentService.Application.Payments.DTO
{
    public class CreatePaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public Guid? OrderId { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public string? Metadata { get; set; }
    }
}