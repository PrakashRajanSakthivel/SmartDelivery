namespace OrderService.Application.Common
{
    public interface IRestaurentService
    {
        Task<bool> ProcessPaymentAsync(string orderId, decimal amount);
    }
}
