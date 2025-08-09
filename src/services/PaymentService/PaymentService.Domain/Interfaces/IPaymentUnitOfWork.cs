using Shared.Data.Interfaces;

namespace PaymentService.Domain.Interfaces
{
    public interface IPaymentUnitOfWork : IUnitOfWork
    {
        IPaymentRepository Payments { get; }
    }
}