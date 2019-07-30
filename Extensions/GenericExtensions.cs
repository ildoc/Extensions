using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class GenericExtensions
    {
        public static bool IsIn<T>(this T value, T[] stringValues) => stringValues.Contains(value);
        public static bool IsIn<T>(this T value, IEnumerable<T> stringValues) => value.IsIn(stringValues.ToArray());
        public static bool IsNotIn<T>(this T value, T[] stringValues) => !value.IsIn(stringValues);
        public static bool IsNotIn<T>(this T value, IEnumerable<T> stringValues) => !value.IsIn(stringValues);
        public static bool IsNotNull<T>(this T value) => !EqualityComparer<T>.Default.Equals(value, default);

        public static T CastTo<T>(this object obj) => (T)Convert.ChangeType(obj, typeof(T));

        public static T AnonymousCastTo<T>(this object o)
        {
            var result = Activator.CreateInstance(typeof(T));
            o.GetType().GetProperties().Each(x => result.GetType().GetProperties().FirstOrDefault(p => p.Name == x.Name)?.SetValue(result, x.GetValue(o)));
            return (T)result;
        }
    }
}
