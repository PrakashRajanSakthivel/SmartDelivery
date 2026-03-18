namespace PaymentService.Domain
{
    public interface IPaymentIntentRepository
    {
        Task<PaymentIntentRecord?> GetByIdAsync(string id);
        Task AddAsync(PaymentIntentRecord record);
    }
}
