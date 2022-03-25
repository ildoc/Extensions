using System;

namespace Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime DayStart(this DateTime date) => date.Date;

        public static DateTime DayEnd(this DateTime date) => date.Date + new TimeSpan(0, 23, 59, 59, 999);

        /// <summary>
        /// Converts DateTime into DateOnly
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateOnly ToDateOnly(this DateTime dateTime) => DateOnly.FromDateTime(dateTime);

        /// <summary>
        /// Converts DateTime into TimeOnly
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static TimeOnly ToTimeOnly(this DateTime dateTime) => TimeOnly.FromDateTime(dateTime);

        /// <summary>
        /// Calculate the age from birthday
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public static int ToAge(this DateTime birthDay) => birthDay.ToAgeAtDate(DateTime.Today);

        /// <summary>
        /// Calculate the age at a specific Date
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public static int ToAgeAtDate(this DateTime birthDay, DateTime atDate)
        {
            var d = atDate.Date;
            var age = d.Year - birthDay.Year;

            // Handles Feb 29th and leap years
            if (birthDay > d.AddYears(-age))
                age--;

            return age;
        }

        /// <summary>
        /// Removes Timezone, Milliseconds and Ticks
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToSimpleDateTime(this DateTime dt) =>
            new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, DateTimeKind.Unspecified);
    }
}
