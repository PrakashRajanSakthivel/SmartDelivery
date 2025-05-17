namespace OrderService.Application.Common
{
    public interface IRestaurentService
    {
        Task<bool> IsPresent(Guid id);
    }
}
