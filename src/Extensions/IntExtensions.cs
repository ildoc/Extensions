using System;

namespace Extensions
{
    public static class IntExtensions
    {
        public static void Times(this int repetitions, Action action)
        {
            for (var i = 0; i < repetitions; i++)
                action();
        }
    }
}
