using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;

namespace Microservices.Common.Types.Abstracts
{
    public abstract class ContextSeedBase<T>
    {
        protected bool IsStoricized(string line, int columnNumber) => line.Split(';')[columnNumber] == "S";
    }

    public static class ContextSeedBaseExtension
    {
        public static Policy CreatePolicy<T>(this ILogger<T> logger, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetry(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(
                            exception,
                            "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                            nameof(T),
                            exception.GetType().Name,
                            exception.Message,
                            retry,
                            retries
                            );
                    }
                );
        }
    }
}
