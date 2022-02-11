using System;
using System.Collections.Generic;
using Extensions;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration) => services.AddMassTransit(configuration, new List<Type>());

        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration, IEnumerable<Type> consumers)
        {
            var options = configuration.GetOptions<MassTransitOptions>("MassTransit");

            services.AddMassTransit(cfg =>
            {
                consumers.Each(c => cfg.AddConsumer(c));

                cfg.UsingRabbitMq((context, cfg) =>
                {

                    cfg.Host(new Uri(options.ServerUri), h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ReceiveEndpoint(configuration.GetSection("App")["Name"], ep =>
                    {
                        consumers.Each(c => ep.ConfigureConsumer(context, c));
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddTransient<IRabbitMqClient, RabbitMqClient>();

            return services;
        }

        public static IApplicationBuilder UseMassTransit(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetService<IBusControl>();
            return app;
        }

        public static Uri GetDestinationAddress(this ConsumeContext context, string queueName)
        {
            var builder = new UriBuilder
            {
                Scheme = context.SourceAddress.Scheme,
                Host = context.SourceAddress.Host,
                Path = queueName
            };

            return builder.Uri;
        }

        public static Uri GetDestinationAddress(this Uri hostAddress, string queueName)
        {
            var builder = new UriBuilder
            {
                Scheme = hostAddress.Scheme,
                Host = hostAddress.Host,
                Path = queueName
            };

            return builder.Uri;
        }
    }
}
