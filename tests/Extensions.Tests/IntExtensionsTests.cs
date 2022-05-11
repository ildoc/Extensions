using FluentAssertions;
using Xunit;

namespace Extensions.Tests
{
    public class IntExtensionsTests
    {
        [Fact]
        public void ShouldExecuteXTimes()
        {
            var count = 0;

            3.Times(() => count++);

            count.Should().Be(3);
        }
    }
}
