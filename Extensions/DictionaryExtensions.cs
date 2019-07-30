using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static void RemoveAllWithAction<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> condition, Action<KeyValuePair<TKey, TValue>> action)
        {
            foreach (var cur in dict.Where(condition).ToList())
            {
                if (action != default) action(cur);
                dict.Remove(cur.Key);
            }
        }

        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> condition) =>
            RemoveAllWithAction(dict, condition, default);

        public static void Each<TKey, TValue>(this Dictionary<TKey, TValue> dict, Action<TKey, TValue> action) =>
            dict.Each(x => action(x.Key, x.Value));

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) =>
            dict.TryGetValue(key, out TValue value) ?
            value :
            throw new KeyNotFoundException($"'{key}' not found in Dictionary");

        public static Dictionary<TValue, TKey> SwapKeyValue<TKey, TValue>(this Dictionary<TKey, TValue> dict) =>
            dict.ToDictionary(x => x.Value, x => x.Key);

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) =>
            key.IsNotNull() && dict.TryGetValue(key, out TValue value) ? value : default;

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict) =>
            dict.ToDictionary(x => x.Key, x => x.Value);
    }
}
