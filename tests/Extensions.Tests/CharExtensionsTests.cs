using Shouldly;
using Xunit;

namespace Extensions.Tests
{
    public class CharExtensionsTests
    {
        [Theory]
        [InlineData('a', 'A')]
        [InlineData('A', 'A')]
        [InlineData(' ', ' ')]
        public void ShouldTransformToUppercase(char value, char expected)
        {
            value.ToUpper().ShouldBe(expected);
        }

        [Theory]
        [InlineData('A', 'a')]
        [InlineData('a', 'a')]
        [InlineData(' ', ' ')]
        public void ShouldTransformToLowercase(char value, char expected)
        {
           value.ToLower().ShouldBe(expected);
        }

        [Theory]
        [InlineData('A', true)]
        [InlineData('a', true)]
        [InlineData(' ', false)]
        public void ShouldReturnIfIsAlphabet(char value, bool expected)
        {
            value.IsAsciiAlphabetLetter().ShouldBe(expected);
        }
    }
}
