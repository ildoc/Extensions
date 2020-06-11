using System;

namespace Extensions
{
    public static class DateTimeExstensions
    {
        public static DateTime DayStart(this DateTime date) => date.Date;
        public static DateTime DayEnd(this DateTime date) => date.Date + new TimeSpan(0, 23, 59, 59, 999);
    }
}
