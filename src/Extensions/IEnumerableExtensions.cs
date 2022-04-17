using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Extensions.Types;

namespace Extensions
{
    public static class IEnumerableExtensions
    {
        //public static void Each<T>(this IEnumerable<T> source, Action<T> action) => For.Each(source, action);

        //public static void Each<T>(this IEnumerable source, Action<T> action) => For.Each(source, action);

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, IEnumerable<T> except, Func<T, T, bool> comparer) =>
            list.Except(except, new KeyEqualityComparer<T>(comparer));

        public static string Join<T>(this IEnumerable<T> source, char separator) => string.Join(separator, source);

        public static string Join<T>(this IEnumerable<T> source, string separator) => string.Join(separator, source);

        public static IEnumerable<T> ConcatIf<T>(this IEnumerable<T> enumerable, bool condition, IEnumerable<T> otherEnumerable) =>
           condition ? enumerable.Concat(otherEnumerable) : enumerable;

        public static IEnumerable<T> ConcatIfElse<T>(this IEnumerable<T> enumerable, bool condition, IEnumerable<T> ifEnumerable, IEnumerable<T> elseEnumerable) =>
           condition ? enumerable.Concat(ifEnumerable) : enumerable.Concat(elseEnumerable);

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> f) =>
            condition ? enumerable.Where(f) : enumerable;

        public static IEnumerable<T> WhereIfElse<T>(this IEnumerable<T> enumerable, bool condition, Func<T, bool> @if, Func<T, bool> @else) =>
            condition ? enumerable.Where(@if) : enumerable.Where(@else);

        /// <summary>
        /// Distinct by specific property.
        /// </summary>
        /// <param name="keySelector">Function to pass object for distinct to work.</param>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        ///   Checks whether all items in the enumerable are same (Uses <see cref="object.Equals(object)" /> to check for equality)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        ///   Returns true if there is 0 or 1 item in the enumerable or if all items in the enumerable are same (equal to
        ///   each other) otherwise false.
        /// </returns>
        public static bool AreAllSame<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            using (var enumerator = enumerable.GetEnumerator())
            {
                var toCompare = default(T);
                if (enumerator.MoveNext())
                {
                    toCompare = enumerator.Current;
                }

                while (enumerator.MoveNext())
                {
                    if (toCompare != null && !toCompare.Equals(enumerator.Current))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        /// <summary>
        ///     For shortcut, no side effects
        /// </summary>
        [DebuggerStepThrough]
        public static void Each<T>(this IEnumerable source, Action<T> action)
        {
            foreach (var item in source)
                action((T)item);
        }

        /// <summary>
        ///     For shortcut with index, no side effects
        /// </summary>
        [DebuggerStepThrough]
        public static void EachIndex<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var i = 0;
            Each(source, item => action(item, i++));
        }
    }
}
