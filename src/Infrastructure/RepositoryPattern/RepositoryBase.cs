using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryPattern
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> _dbSet { get; }

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Query(bool trackChanges = true) =>
            !trackChanges ?
              _dbSet.AsNoTracking() :
              _dbSet;

        public virtual T GetById<TId>(TId id, bool track = true)
        {
            var entity = _dbSet.Find(id);
            if (!track)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<T> GetByIdAsync<TId>(TId id, bool track = true)
        {
            var entity = await _dbSet.FindAsync(id);
            if (!track)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void Create(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);
        public void UpdateRange(params T[] entities) => _dbSet.UpdateRange(entities);
        public void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);

        public void Remove(T entity) => _dbSet.Remove(entity);
        public void RemoveRange(params T[] entities) => _dbSet.RemoveRange(entities);
        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public virtual async void RemoveByIdAsync<TId>(TId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityToDelete = await GetByIdAsync(id);

            if (entityToDelete == null)
                throw new Exception("Entity not found");

            _dbSet.Remove(entityToDelete);
        }

        public virtual void RemoveById<TId>(TId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityToDelete = GetById(id);

            if (entityToDelete == null)
                throw new Exception("Entity not found");

            _dbSet.Remove(entityToDelete);
        }
    }
}
