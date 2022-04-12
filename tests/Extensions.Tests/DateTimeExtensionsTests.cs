using System;
using Xunit;

namespace Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void DayStartMinusOneMillisecondIsPreviousDay()
        {
            var date = DateTime.Now;

            Assert.True(date.DayStart().AddMilliseconds(-1).Date == date.AddDays(-1).Date);
        }

        [Fact]
        public void DayEndPlusOneMillisecondIsNextDay()
        {
            var date = DateTime.Now;

            Assert.True(date.DayEnd().AddMilliseconds(1).Date == date.AddDays(1).Date);
        }

        [Fact]
        public void ShouldCalculateAgeOnSpecificDate()
        {
            var birthday = new DateTime(1990, 6, 22);
            Assert.Equal(31, birthday.ToAgeAtDate(new DateTime(2022, 4, 12)));
        }
    }
}
