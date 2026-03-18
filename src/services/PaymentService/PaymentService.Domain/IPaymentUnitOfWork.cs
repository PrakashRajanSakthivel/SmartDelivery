using Shared.Data.Interfaces;

namespace PaymentService.Domain
{
    public interface IPaymentUnitOfWork : IUnitOfWork
    {
        IPaymentIntentRepository PaymentIntents { get; }
    }
}
