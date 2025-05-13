using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Interfaces;
using System.Linq.Expressions;


namespace Shared.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public virtual async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        public virtual void Update(T entity)
            => _dbSet.Update(entity);

        public virtual void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

        public virtual void Remove(T entity)
            => _dbSet.Remove(entity);

        public virtual void RemoveRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        public virtual async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
