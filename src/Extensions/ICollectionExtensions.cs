using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class ICollectionExtensions
    {
        public static bool AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T value)
        {
            if (predicate(value))
            {
                collection.Add(value);
                return true;
            }

            return false;
        }

        public static bool AddIfNotContains<T>(this ICollection<T> collection, T value)
        {
            if (!collection.Contains(value))
            {
                collection.Add(value);
                return true;
            }

            return false;
        }
    }
}
