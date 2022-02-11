using System;
using System.Drawing;

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

        public static string NextHexColor(this Random random)
        {
            var color = random.NextColor();
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static Color NextColor(this Random random)
        {
            var colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            return Color.FromKnownColor(colors[random.Next(colors.Length)]);
        }

        public static bool CoinToss(this Random random)
        {
            return random.Next(2) == 0;
        }
    }
}
