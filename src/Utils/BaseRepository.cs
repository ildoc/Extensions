using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Utils
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities);
        IQueryable<T> GetAll();
        Task<T> Update(T entity);
        Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities);
        Task<T> Remove(T entity);
        Task<IEnumerable<T>> RemoveRange(IEnumerable<T> entities);
    }

    public abstract class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly DbContext _context;

        protected BaseRepository(DbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual async Task<T> Remove(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> RemoveRange(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<T> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
    }
}
