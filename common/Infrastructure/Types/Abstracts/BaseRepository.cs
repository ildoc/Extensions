using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Authorization;
using Infrastructure.Types.Base;
using Infrastructure.Types.Interfaces;

namespace Infrastructure.Types.Abstracts
{
    public abstract class BaseRepository<T> : Utils.BaseRepository<T>, IRepository<T> where T : class, new()
    {
        private readonly BaseDbContext _context;

        protected BaseRepository(BaseDbContext context) : base(context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T entity, UserInfo userInfo = null)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync(userInfo);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, UserInfo userInfo = null)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync(userInfo);
            return entities;
        }

        public virtual async Task<T> UpdateAsync(T entity, UserInfo userInfo = null)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(userInfo);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, UserInfo userInfo = null)
        {
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync(userInfo);
            return entities;
        }
    }
}
