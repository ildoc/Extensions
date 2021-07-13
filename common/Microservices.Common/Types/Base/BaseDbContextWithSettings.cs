using Microservices.Common.Settings;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Common.Types.Base
{
    public class BaseDbContextWithSettings : BaseDbContext, IDbContextWithSettings
    {
        protected BaseDbContextWithSettings(DbContextOptions options) : base(options) { }

        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    }
}
