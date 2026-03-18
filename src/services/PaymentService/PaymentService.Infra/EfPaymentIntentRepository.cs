using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;

namespace PaymentService.Infra
{
    public class EfPaymentIntentRepository : IPaymentIntentRepository
    {
        private readonly PaymentDbContext _context;

        public EfPaymentIntentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentIntentRecord?> GetByIdAsync(string id)
        {
            return await _context.PaymentIntents.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(PaymentIntentRecord record)
        {
            await _context.PaymentIntents.AddAsync(record);
        }
    }
}
