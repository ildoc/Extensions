using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.Utils;

namespace Extensions
{
    public static class IEnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> source, Action<T> action) => For.Each(source, action);

        public static void Each<T>(this IEnumerable source, Action<T> action) => For.Each(source, action);
    }
}
