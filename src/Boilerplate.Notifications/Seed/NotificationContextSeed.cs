using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Common.Settings;
using Boilerplate.Common.Types.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Boilerplate.Notifications.Settings;

namespace Boilerplate.Notifications.Seed
{
    public class NotificationContextSeed : ContextSeedBase<NotificationContextSeed>
    {
        private readonly ILogger<NotificationContextSeed> _logger;
        private readonly NotificationContext _context;
        private readonly IWebHostEnvironment _env;

        public NotificationContextSeed(NotificationContext context, ILogger<NotificationContextSeed> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;
        }

        public async Task SeedAsync()
        {
            var policy = _logger.CreatePolicy();

            await policy.Execute(async () =>
            {
                if (_env.IsDevelopment())
                {
                    if (_context.ApplicationSettings.FirstOrDefault(x => x.Key == NotificationApplicationSettings.NOTIFICATIONS_EXPIRE_AFTER) == default)
                    {
                        var setting = new ApplicationSetting
                        {
                            Key = NotificationApplicationSettings.NOTIFICATIONS_EXPIRE_AFTER,
                            Value = "20"
                        };

                        await _context.ApplicationSettings.AddAsync(setting);
                        await _context.SaveChangesAsync();
                    }
                }
            });
        }
    }
}
