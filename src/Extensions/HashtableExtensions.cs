using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class HashtableExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this Hashtable table) =>
            table.Cast<DictionaryEntry>()
            .ToDictionary(kvp => (TKey)kvp.Key, kvp => (TValue)kvp.Value);
    }
}
