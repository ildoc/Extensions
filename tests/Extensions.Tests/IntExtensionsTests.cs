﻿using Xunit;

namespace Extensions.Tests
{
    public class IntExtensionsTests
    {
        [Fact]
        public void ShouldExecuteXTimes()
        {
            var count = 0;

            3.Times(() => count++);

            Assert.Equal(3, count);
        }
    }
}
