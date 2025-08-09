namespace PaymentService.Domain.Entites
{
    public enum PaymentStatus
    {
        // Initial states
        Created,
        RequiresPaymentMethod,
        
        // Processing states
        Processing,
        RequiresConfirmation,
        
        // Final states
        Succeeded,
        Failed,
        Canceled,
        
        // Error states
        RequiresAction,
        PaymentFailed,
        
        // Refund states
        PartiallyRefunded,
        Refunded
    }
}