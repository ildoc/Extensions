using Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RepositoryPattern
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private DbSet<T> DbSet { get; }

        public RepositoryBase(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public IQueryable<T> Query(bool trackChanges = true) =>
              DbSet.AsNoTrackingIf(!trackChanges);

        public virtual T GetById<TId>(TId id, bool track = true)
        {
            var entity = DbSet.Find(id);
            if (!track)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<T> GetByIdAsync<TId>(TId id, bool track = true)
        {
            var entity = await DbSet.FindAsync(id);
            if (!track)
                _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void Create(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);
        public void UpdateRange(params T[] entities) => DbSet.UpdateRange(entities);
        public void UpdateRange(IEnumerable<T> entities) => DbSet.UpdateRange(entities);

        public void Remove(T entity) => DbSet.Remove(entity);
        public void RemoveRange(params T[] entities) => DbSet.RemoveRange(entities);
        public void RemoveRange(IEnumerable<T> entities) => DbSet.RemoveRange(entities);

        public virtual async void RemoveByIdAsync<TId>(TId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityToDelete = await GetByIdAsync(id);

            if (entityToDelete == null)
                throw new Exception("Entity not found");

            DbSet.Remove(entityToDelete);
        }

        public virtual void RemoveById<TId>(TId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityToDelete = GetById(id);

            if (entityToDelete == null)
                throw new Exception("Entity not found");

            DbSet.Remove(entityToDelete);
        }
    }
}
