using Boilerplate.Common.Types.Base;
using Boilerplate.Notifications.Models;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Notifications
{
    public class NotificationContext : BaseDbContextWithSettings
    {
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Guid);
            });
        }
    }
}
