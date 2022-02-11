using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Common.Authorization;
using Boilerplate.Common.Types.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Boilerplate.Common.Types.Base
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions options) : base(options) { }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await SaveChangesAsync(null, cancellationToken);

        public async Task<int> SaveChangesAsync(UserInfo userInfo, CancellationToken cancellationToken = default)
        {
            AddCreationAndModifiedDate(userInfo);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges() => SaveChanges(null);

        public int SaveChanges(UserInfo userInfo)
        {
            AddCreationAndModifiedDate(userInfo);
            return base.SaveChanges();
        }

        private void AddCreationAndModifiedDate(UserInfo user)
        {
            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            addedEntities.ForEach(e =>
            {
                if (typeof(ISystemEntity).IsAssignableFrom(e.Entity.GetType()))
                {
                    ((ISystemEntity)e.Entity).CreationDate = DateTime.UtcNow;
                    ((ISystemEntity)e.Entity).CreatedById ??= user?.UserId;
                    ((ISystemEntity)e.Entity).CreatedByUsername ??= user?.TaxCode;
                }
            });

            var editedEntities = ChangeTracker.Entries<ISystemEntity>().Where(e => e.State == EntityState.Modified).ToList();

            editedEntities.ForEach(e =>
                {
                    e.Property(x => x.CreationDate).IsModified = false;
                    e.Property(x => x.CreatedById).IsModified = false;
                    e.Property(x => x.CreatedByUsername).IsModified = false;

                    e.Entity.ModifiedDate = DateTime.UtcNow;
                    e.Entity.ModifiedById ??= user?.UserId;
                    e.Entity.ModifiedByUsername ??= user?.TaxCode;
                });
        }
    }
}
