﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Extensions
{
    public static class GenericExtensions
    {
        public static bool IsIn<T>(this T value, T[] values) => values.Contains(value);
        public static bool IsIn<T>(this T value, IEnumerable<T> values) => value.IsIn(values.ToArray());
        public static bool IsNotIn<T>(this T value, T[] stringValues) => !value.IsIn(stringValues);
        public static bool IsNotIn<T>(this T value, IEnumerable<T> stringValues) => !value.IsIn(stringValues);
        public static bool IsNull<T>(this T value) => EqualityComparer<T>.Default.Equals(value, default);
        public static bool IsNotNull<T>(this T value) => !EqualityComparer<T>.Default.Equals(value, default);

        public static T CastTo<T>(this object obj) => (T)Convert.ChangeType(obj, typeof(T));

        public static T AnonymousCastTo<T>(this object o)
        {
            var result = Activator.CreateInstance(typeof(T));
            o.GetType().GetProperties().Each(x => result.GetType().GetProperties().FirstOrDefault(p => p.Name == x.Name)?.SetValue(result, x.GetValue(o)));
            return (T)result;
        }

        public static T Clone<T>(this T _this) => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(_this));

        public static T DeepClone<T>(this T obj) where T : ISerializable
        {
            T objResult;
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Position = 0;
                objResult = (T)bf.Deserialize(ms);
            }
            return objResult;
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
    }
}
