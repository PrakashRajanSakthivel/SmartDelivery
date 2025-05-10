using OrderService.Domain.Entites;

namespace OrderService.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid orderId);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task SaveChangesAsync();
    }
}
