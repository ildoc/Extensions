using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Extensions.Tests
{
    public class RandomExtensionsTests
    {
        [Fact]
        public void ShouldGenerateRandomDateTimeWithinRange()
        {
            var r = new Random();

            var dateList = new List<DateTime>();
            for (var i = 0; i < 100; i++)
                dateList.Add(r.NextDatetime(new DateTime(2010, 1, 1), new DateTime(2020, 1, 1)));

            dateList.Should().Contain(x => x >= new DateTime(2010, 1, 1) && x <= new DateTime(2020, 1, 1));
        }

        [Fact]
        public void ShouldPickOneOfList()
        {
            var r = new Random();
            var list = new List<int>();

            for (var i = 0; i < 100; i++)
                list.Add(r.Next());

            var one = r.OneOf(list);

            list.Should().Contain(one);
        }
    }
}
