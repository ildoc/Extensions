using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class RangeExtensions
    {
        public static IEnumerator<int> GetEnumerator(this Range range)
        {
            var start = range.Start.GetOffset(int.MaxValue);
            var end = range.End.GetOffset(int.MaxValue);
            var acc = end.CompareTo(start);
            var current = start;
            do
            {
                yield return current;
                current += acc;
            } while (current != end);
        }
    }
}
