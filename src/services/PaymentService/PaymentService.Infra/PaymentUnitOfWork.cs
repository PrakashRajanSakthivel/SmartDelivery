using PaymentService.Domain;
using Shared.Data.Repositories;

namespace PaymentService.Infra
{
    public class PaymentUnitOfWork : UnitOfWork<PaymentDbContext>, IPaymentUnitOfWork
    {
        public IPaymentIntentRepository PaymentIntents { get; }

        public PaymentUnitOfWork(PaymentDbContext context, IPaymentIntentRepository paymentIntents) : base(context)
        {
            PaymentIntents = paymentIntents;
        }
    }
}
