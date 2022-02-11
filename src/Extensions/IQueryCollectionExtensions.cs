using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Extensions
{
    public static class IQueryCollectionExtensions
    {
        public static StringValues GetValueOrDefault(this IQueryCollection queryCollection, string key) =>
            queryCollection.TryGetValue(key, out var result) ? result : default;
    }
}
