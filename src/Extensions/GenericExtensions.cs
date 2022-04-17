using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Extensions
{
    public static class GenericExtensions
    {
        public static bool IsIn<T>(this T value, T[] values) => values.Contains(value);
        public static bool IsIn<T>(this T value, IEnumerable<T> values) => value.IsIn(values.ToArray());
        public static bool IsNotIn<T>(this T value, T[] values) => !value.IsIn(values);
        public static bool IsNotIn<T>(this T value, IEnumerable<T> values) => !value.IsIn(values);
        public static bool IsNull<T>(this T value) => EqualityComparer<T>.Default.Equals(value, default);
        public static bool IsNotNull<T>(this T value) => !EqualityComparer<T>.Default.Equals(value, default);

        public static T CastTo<T>(this object obj) => (T)Convert.ChangeType(obj, typeof(T));

        public static T AnonymousCastTo<T>(this object o)
        {
            var result = Activator.CreateInstance(typeof(T));
            o.GetType().GetProperties().Each(x => result.GetType().GetProperties().FirstOrDefault(p => p.Name == x.Name)?.SetValue(result, x.GetValue(o)));
            return (T)result;
        }

        public static T Clone<T>(this T obj) => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));

        public static T DeepClone<T>(this T obj) where T : ISerializable
        {
            using var ms = new MemoryStream();

            var bf = new DataContractSerializer(typeof(T));
            bf.WriteObject(ms, obj);
            ms.Position = 0;
            return (T)bf.ReadObject(ms);
        }

        public static bool ToBool<T>(this T o) => o switch
        {
            bool b => b,
            int i => i != default,
            string s => bool.TryParse(s, out var res) && res,
            float f => f != default,
            double d => d != default,
            _ => o != null
        };

#nullable enable
        public static bool IsEqualTo(this object? a, object? b)
        {
            if (a == default && b != default)
                return false;
            if (a == default && b == default)
                return true;
            return a?.Equals(b) ?? false;
        }
#nullable disable

        public static bool Between<T>(this T item, T start, T end, bool includeStart = true, bool includeEnd = true)
        {
            return
                (
                    (includeStart && Comparer<T>.Default.Compare(item, start) >= 0)
                    ||
                    (!includeStart && Comparer<T>.Default.Compare(item, start) > 0)
                )
                &&
                (
                    (includeEnd && Comparer<T>.Default.Compare(item, end) <= 0)
                    ||
                    (!includeEnd && Comparer<T>.Default.Compare(item, end) < 0)
                );
        }

        public static List<Variance> DetailedCompare<T>(this T val1, T val2)
        {
            var variances = new List<Variance>();
            foreach (var f in val1.GetType().GetFields())
            {
                var v = new Variance
                {
                    Prop = f.Name,
                    ValA = f.GetValue(val1),
                    ValB = f.GetValue(val2)
                };
                if (!v.ValA.IsEqualTo(v.ValB))
                    variances.Add(v);
            }

            foreach (var p in val1.GetType().GetProperties())
            {
                var v = new Variance
                {
                    Prop = p.Name,
                    ValA = p.GetValue(val1),
                    ValB = p.GetValue(val2)
                };
                if (!v.ValA.IsEqualTo(v.ValB))
                    variances.Add(v);
            }
            return variances;
        }
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object ValA { get; set; }
        public object ValB { get; set; }
    }
}
