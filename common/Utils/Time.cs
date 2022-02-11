using System;
using System.Globalization;

namespace Utils
{
    public static class Time
    {
        public const string F_ISO8601 = "yyyy-MM-dd HH:mm:ss.fff";
        public static string GetTimestamp() => Now.ToString("yyyyMMddTHHmmssfff");
        public static string GetFilename() => Now.ToString("yyyyMMddTHHmmssfff");
        public static DateTime Now => DateTime.UtcNow;
        public static string NowAsString => ToString(Now);
        public static string ToString(in DateTime d) => d.ToString(F_ISO8601, CultureInfo.InvariantCulture);
        public static DateTime Parse(string s) => DateTime.SpecifyKind(DateTime.ParseExact(s, F_ISO8601, CultureInfo.InvariantCulture), DateTimeKind.Utc);
        public static DateTime MaxValue => DateTime.MaxValue;
        public static DateTime MinValue => DateTime.MinValue;
    }
}
