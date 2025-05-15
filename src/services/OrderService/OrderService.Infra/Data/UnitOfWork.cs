using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Domain.Interfaces;
using Shared.Data.Repositories;

namespace OrderService.Infra.Data
{
    // services/OrderService/OrderService.Infra/Data/OrderUnitOfWork.cs
    public class OrderUnitOfWork : UnitOfWork<OrderDbContext>, IOrderUnitOfWork
    {
        public IOrderRepository Orders { get; }
        public OrderUnitOfWork(
            OrderDbContext context,
            IOrderRepository orders) : base(context)
        {
            Orders = orders;
        }
    }
}
