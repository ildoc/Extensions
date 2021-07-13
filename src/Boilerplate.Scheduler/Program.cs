using Boilerplate.Common.MassTransit;
using Microsoft.Extensions.Hosting;

namespace Boilerplate.Scheduler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(hostContext.Configuration);
                    services.AddDependencies(hostContext.Configuration);
                });
    }
}
