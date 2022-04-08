using Infrastructure.Types.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.RepositoryPattern
{
    public abstract class UnitOfWorkBase<TDbContext> : IDisposable, IUnitOfWorkBase where TDbContext : DbContext
    {
        protected readonly TDbContext _context;
        private readonly IServiceScope _scope;
        private readonly Dictionary<Type, IRepository> _repositories = new();

        protected UnitOfWorkBase(TDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scope = scopeFactory.CreateScope();
        }

        protected IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            IRepository<TEntity> value;

            if (_repositories.ContainsKey(type))
            {
                value = (IRepository<TEntity>)_repositories[type];
            }
            else
            {
                value = _scope.ServiceProvider.GetService<IRepository<TEntity>>();
                _repositories.Add(type, value);
            }
            return value;
        }

        public void Dispose()
        {
            _scope?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Save() => _context.SaveChanges();
        public Task<int> SaveAsync() => _context.SaveChangesAsync();
    }

    public abstract class UnitOfWorkBase<TDbContext, TUser> : UnitOfWorkBase<TDbContext>, IUnitOfWorkBase<TUser>
        where TDbContext : BaseIdentityDbContext<TUser>
        where TUser : IdentityUser
    {
        protected UnitOfWorkBase(TDbContext context, IServiceScopeFactory scopeFactory) : base(context, scopeFactory)
        {
        }

        public void Save(TUser user) => _context.SaveChanges(user);
        public Task<int> SaveAsync(TUser user) => _context.SaveChangesAsync(user);
    }
}
