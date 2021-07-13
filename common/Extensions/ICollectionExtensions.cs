using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void TryUpdateManyToMany<T>(this ICollection<T> currentItems, ICollection<T> newItems, Func<T,T,bool> comparer) where T : class
        {
            var toRemove = currentItems.Except(newItems, comparer).ToList();
            var toAdd = newItems.Except(currentItems, comparer).ToList();

            foreach (var item in toRemove)
            {
                currentItems.Remove(item);
            }

            foreach (var item in toAdd)
            {
                currentItems.Add(item);
            }
        }
    }
}
