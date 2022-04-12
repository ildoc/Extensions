namespace Infrastructure.RepositoryPattern
{
    public interface IRepository<T> : IRepository where T : class
    {
        void Create(T entity);
        T GetById<TId>(TId id, bool track = true);
        Task<T> GetByIdAsync<TId>(TId id, bool track = true);
        IQueryable<T> Query(bool trackChanges = true);
        void Remove(T entity);
        void RemoveById<TId>(TId id);
        void RemoveByIdAsync<TId>(TId id);
        void RemoveRange(IEnumerable<T> entities);
        void RemoveRange(params T[] entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void UpdateRange(params T[] entities);
    }

    public interface IRepository
    {

    }
}
