using CartService.Domain.Interfaces;
using Shared.Data.Repositories;

namespace CartService.Infra.Data
{
    // services/OrderService/OrderService.Infra/Data/OrderUnitOfWork.cs
    public class CartUnitOfWork : UnitOfWork<CartDbContext>, ICartUnitOfWork
    {
        public ICartRepository Carts { get; }

        public CartUnitOfWork(CartDbContext context, ICartRepository carts) : base(context)
        {
            Carts = carts;
        }
    }
}
