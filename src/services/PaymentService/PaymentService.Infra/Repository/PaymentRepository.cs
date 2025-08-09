using PaymentService.Domain.Entites;
using PaymentService.Domain.Interfaces;
using PaymentService.Infra.Data;
using Shared.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Infra.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public async Task<IEnumerable<Payment>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetFailedPaymentsForRetryAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == PaymentStatus.Failed && 
                           p.RetryCount < 3 && 
                           (p.LastRetryAt == null || 
                            p.LastRetryAt < DateTime.UtcNow.AddMinutes(-30)))
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}