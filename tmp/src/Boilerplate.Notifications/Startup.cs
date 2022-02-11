using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Common.Exceptions;
using Boilerplate.Common.Health;
using Boilerplate.Common.Keycloak;
using Boilerplate.Common.Logging;
using Boilerplate.Common.MassTransit;
using Boilerplate.Common.Settings;
using Boilerplate.Notifications.Consumers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Boilerplate.Notifications
{
    public class Startup
    {
        private readonly bool _isDevelopment;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();
            _isDevelopment = env.IsDevelopment();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        private readonly List<Type> _masstransitConsumers = new List<Type> {
            typeof(UserPrimaryDataChangedConsumer),
        };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog(Configuration);
            services.AddCors();
            services.AddApplicationSettings();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;

                    o.Authority = $"{Configuration["AuthSettings:KeycloakBaseUrl"]}realms/{Configuration["AuthSettings:RealmName"]}";
                    o.Audience = Configuration["AuthSettings:WebAppClientId"];

                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();

                            //c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            if (_isDevelopment)
                            {
                                return c.Response.WriteAsync(c.Exception.ToString());
                            }
                            return c.Response.WriteAsync("An error occured processing your authentication.");
                        },
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/ws/notificationhub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                if (_isDevelopment)
                    options.PayloadSerializerSettings.Formatting = Formatting.Indented;
            });
            services.AddDependencies(Configuration);
            services.AddKeycloackClient(Configuration);
            services.AddMassTransit(Configuration, _masstransitConsumers);

            services.AddCustomHealthCheck(Configuration, "NotificationContext");
            services.AddControllersWithExceptionFilters()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                    if (_isDevelopment)
                        options.SerializerSettings.Formatting = Formatting.Indented;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            if (!_isDevelopment)
            {
                app.Use((context, next) => { context.Request.Scheme = "https"; return next(); });
            }

            app.UseCors(builder =>
               builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
            );

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseMassTransit();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/ws/notificationhub");
                endpoints.MapCustomHealthChecks();
            });
        }
    }
}
