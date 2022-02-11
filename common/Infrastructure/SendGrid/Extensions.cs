using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace Infrastructure.SendGrid
{
    public static class Extensions
    {
        public static IServiceCollection AddSendGrid(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSendGrid(options =>
            {
                options.ApiKey = configuration["SendGridSettings:ApiKey"];
            });

            services.AddTransient<IEmailManager, EmailManager>();

            return services;
        }
    }
}
