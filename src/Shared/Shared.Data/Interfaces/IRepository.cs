using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Shared.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        Task<int> SaveChangesAsync();
    }
}
