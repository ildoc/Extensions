using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Types.Base
{
    public interface IDbContextWithSettings
    {
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    }
}
