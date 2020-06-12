using Xunit;

namespace Extensions.Tests
{
    public class GenericExtensionsTests
    {
        [Theory]
        [InlineData('b', "abcd", true)]
        [InlineData('f', "abcd", false)]
        public void ShouldCheckIfValueIsInString(char value, string fullString, bool expected)
        {
            Assert.Equal(expected, value.IsIn(fullString));
        }

        [Theory]
        [InlineData('b', "abcd", false)]
        [InlineData('f', "abcd", true)]
        public void ShouldCheckIfValueIsNotInString(char value, string fullString, bool expected)
        {
            Assert.Equal(expected, value.IsNotIn(fullString));
        }
    }
}
