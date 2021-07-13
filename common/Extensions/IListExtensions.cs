using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Extensions
{
    public static class IListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do
                    provider.GetBytes(box);
                while (box[0] >= n * (byte.MaxValue / n));
                var k = (box[0] % n);
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.ConvertAll(item => (T)item.Clone());
        }
    }
}
