using Shared.Data.Interfaces;


namespace CartService.Domain.Interfaces
{
    // services/OrderService/OrderService.Infra/Data/IOrderUnitOfWork.cs
    public interface ICartUnitOfWork : IUnitOfWork
    {
        ICartRepository Carts { get; }
    }

    
}
