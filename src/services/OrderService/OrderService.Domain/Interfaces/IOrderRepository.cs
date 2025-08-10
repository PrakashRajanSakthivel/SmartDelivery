using OrderService.Domain.Entites;
using Shared.Data.Interfaces;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId);
    }
}
