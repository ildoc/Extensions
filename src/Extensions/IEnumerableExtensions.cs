using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Extensions
{
    public static class IEnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> source, Action<T> action) => For.Each(source, action);

        public static void Each<T>(this IEnumerable source, Action<T> action) => For.Each(source, action);

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, IEnumerable<T> except, Func<T, T, bool> comparer) =>
            list.Except(except, new KeyEqualityComparer<T>(comparer));

        public static string Join<T>(this IEnumerable<T> source, char separator) => string.Join(separator, source);
        public static string Join<T>(this IEnumerable<T> source, string separator) => string.Join(separator, source);

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> f) =>
           condition ? enumerable.Where(f) : enumerable;

        public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> enumerable, bool condition, IEnumerable<T> otherEnumerable) =>
           condition ? enumerable.Concat(otherEnumerable) : enumerable;

        public static IEnumerable<T> ConcatIfElse<T>(this IEnumerable<T> enumerable, bool condition, IEnumerable<T> ifEnumerable, IEnumerable<T> elseEnumerable) =>
           condition ? enumerable.Concat(ifEnumerable) : enumerable.Concat(elseEnumerable);
    }
}
