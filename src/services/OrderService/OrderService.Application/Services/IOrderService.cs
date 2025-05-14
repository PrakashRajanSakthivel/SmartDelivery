using OrderService.Application.Model;

namespace OrderService.Application.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(CreateOrderRequest request);
    }

}
