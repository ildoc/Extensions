using Boilerplate.Common.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Boilerplate.Common.Logging
{
    public static class Extensions
    {
        public static IServiceCollection AddSerilog(this IServiceCollection _this, IConfiguration configuration)
        {
            _this.AddLogging(configure => configure.AddSerilog(CreateSerilogLogger(configuration)));

            return _this;
        }

        public static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            //var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            //var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", configuration["App:Name"])
                .Enrich.FromLogContext()
                //.WriteTo.Console()
                //.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                //.WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public static void UserLog<T>(this ILogger<T> logger, string message, UserInfo userInfo) =>
            logger.LogInformation($"User {userInfo.TaxCode}({userInfo.UserId}) - {message}");
    }
}
