using System;

namespace Extensions
{
    public static class RandomExtensions
    {
        public static DateTime NextDatetime(this Random random) => NextDatetime(random, new DateTime(1970, 1, 1), new DateTime(2099, 1, 1));

        public static DateTime NextDatetime(this Random random, in DateTime from, in DateTime to) =>
             from + new TimeSpan((long)(random.NextDouble() * (to - from).Ticks));

        public static T OneOf<T>(this Random random, params T[] values)
        {
            return values[random.Next(values.Length)];
        }

        public static bool CoinToss(this Random random)
        {
            return random.Next(2) == 0;
        }
    }
}
