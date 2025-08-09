using PaymentService.Domain.Interfaces;
using Shared.Data.Repositories;

namespace PaymentService.Infra.Data
{
    public class PaymentUnitOfWork : UnitOfWork<PaymentDbContext>, IPaymentUnitOfWork
    {
        public IPaymentRepository Payments { get; }
        
        public PaymentUnitOfWork(
            PaymentDbContext context,
            IPaymentRepository payments) : base(context)
        {
            Payments = payments;
        }
    }
}