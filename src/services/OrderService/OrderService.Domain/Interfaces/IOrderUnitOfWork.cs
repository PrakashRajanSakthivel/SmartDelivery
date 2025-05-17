using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Data.Interfaces;
using Shared.Data.Repositories;

namespace OrderService.Domain.Interfaces
{
    // services/OrderService/OrderService.Infra/Data/IOrderUnitOfWork.cs
    public interface IOrderUnitOfWork : IUnitOfWork
    {
        IOrderRepository Orders { get; }
    }

    
}
