using System;
using Shouldly;
using Xunit;

namespace Extensions.Tests
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void DayStartMinusOneMillisecondIsPreviousDay()
        {
            var date = DateTime.Now;

            date.DayStart().AddMilliseconds(-1).Date.ShouldBe(date.AddDays(-1).Date);
        }

        [Fact]
        public void DayEndPlusOneMillisecondIsNextDay()
        {
            var date = DateTime.Now;

            date.DayEnd().AddMilliseconds(1).Date.ShouldBe(date.AddDays(1).Date);
        }

        [Fact]
        public void ShouldCalculateAgeOnSpecificDate()
        {
            var birthday = new DateTime(1990, 6, 22);

            birthday.ToAgeAtDate(new DateTime(2022, 4, 12)).ShouldBe(31);
        }
    }

    public class ObjectExtensionsTests
    {
        [Theory]
        [InlineData(false, false)]
        [InlineData(null, false)]
        [InlineData("lallallero", false)]
        [InlineData(123, true)]
        [InlineData(1.3, true)]
        [InlineData(-123, true)]
        [InlineData(-1.3, true)]
        public void ShouldCheckIfNumber(object o, bool expected)
        {
            o.IsNumber().ShouldBe(expected);
        }
    }
}
