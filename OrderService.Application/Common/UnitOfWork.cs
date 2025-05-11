using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Infra.Data;

namespace OrderService.Application.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _context;

        public UnitOfWork(OrderDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }

}
