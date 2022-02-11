using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Types.Base
{
    public class BaseDbContextWithSettings : BaseDbContext, IDbContextWithSettings
    {
        protected BaseDbContextWithSettings(DbContextOptions options) : base(options) { }

        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    }
}
