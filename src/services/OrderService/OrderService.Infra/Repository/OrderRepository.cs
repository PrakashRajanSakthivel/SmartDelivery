using OrderService.Domain.Entites;
using OrderService.Domain.Interfaces;
using OrderService.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Repositories;

namespace OrderService.Infra.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
