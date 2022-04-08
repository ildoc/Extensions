using Infrastructure.Types.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Types.Base
{
    public abstract class BaseIdentityDbContext<TUser> : IdentityDbContext<TUser> where TUser : IdentityUser
    {
        protected BaseIdentityDbContext(DbContextOptions options) : base(options) { }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await SaveChangesAsync(null, cancellationToken);

        public async Task<int> SaveChangesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            AddCreationAndModifiedDate(user);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges() => SaveChanges(null);

        public int SaveChanges(TUser user)
        {
            AddCreationAndModifiedDate(user);
            return base.SaveChanges();
        }

        private void AddCreationAndModifiedDate(TUser user)
        {
            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();

            addedEntities.ForEach(e =>
            {
                if (typeof(ISystemEntity).IsAssignableFrom(e.Entity.GetType()))
                {
                    ((ISystemEntity)e.Entity).CreationDate = DateTime.UtcNow;
                    ((ISystemEntity)e.Entity).CreatedById ??= user?.Id;
                    ((ISystemEntity)e.Entity).CreatedByUsername ??= user?.UserName;
                }
            });

            var editedEntities = ChangeTracker.Entries<ISystemEntity>().Where(e => e.State == EntityState.Modified).ToList();

            editedEntities.ForEach(e =>
            {
                e.Property(x => x.CreationDate).IsModified = false;
                e.Property(x => x.CreatedById).IsModified = false;
                e.Property(x => x.CreatedByUsername).IsModified = false;

                e.Entity.ModifiedDate = DateTime.UtcNow;
                e.Entity.ModifiedById ??= user?.Id;
                e.Entity.ModifiedByUsername ??= user?.UserName;
            });
        }
    }
}
