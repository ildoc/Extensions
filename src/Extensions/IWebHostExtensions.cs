using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Extensions
{
    public static class IWebHostExtensions
    {
        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost) where TContext : DbContext
        {
            using var scope = webHost.Services.CreateScope();

            TryMigrate<TContext>(scope);

            return webHost;
        }

        public static Microsoft.Extensions.Hosting.IHost MigrateDbContext<TContext>(this Microsoft.Extensions.Hosting.IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            TryMigrate<TContext>(scope);

            return host;
        }

        public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder = default) where TContext : DbContext
        {
            using var scope = webHost.Services.CreateScope();

            TryMigrateAndSeed(scope, seeder);

            return webHost;
        }

        public static Microsoft.Extensions.Hosting.IHost MigrateDbContext<TContext>(this Microsoft.Extensions.Hosting.IHost host, Action<TContext, IServiceProvider> seeder = default) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            TryMigrateAndSeed(scope, seeder);

            return host;
        }

        private static void TryMigrateAndSeed<TContext>(IServiceScope scope, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);


                var retry = Policy.Handle<SqlException>()
                     .WaitAndRetry(new TimeSpan[]
                     {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                     });

                //if the sql server container is not created on run docker compose this
                //migration can't fail for network related exception. The retry options for DbContext only 
                //apply to transient exceptions
                retry.Execute(() => InvokeSeeder(seeder, context, services));

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }
        }

        private static void TryMigrate<TContext>(IServiceScope scope) where TContext : DbContext
        {
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);


                var retry = Policy.Handle<SqlException>()
                     .WaitAndRetry(new TimeSpan[]
                     {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                     });

                //if the sql server container is not created on run docker compose this
                //migration can't fail for network related exception. The retry options for DbContext only 
                //apply to transient exceptions
                retry.Execute(() => context.Database.Migrate());

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
