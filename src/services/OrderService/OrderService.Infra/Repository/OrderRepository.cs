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

      
    }

}
