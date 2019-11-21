using System;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static T TryParse<T>(this Enum e, string s) where T : Enum
        {
            Enum.TryParse(e.GetType(), s, out var result);
            return (T)result;
        }
    }
}
