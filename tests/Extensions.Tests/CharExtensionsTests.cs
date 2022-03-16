using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Extensions.Tests
{
    public class CharExtensionsTests
    {
        [Theory]
        [InlineData('a','A')]
        [InlineData('A','A')]
        [InlineData(' ',' ')]
        public void ShouldTransformToUppercase(char value, char expected)
        {
            Assert.Equal(expected, value.ToUpper());
        }

        [Theory]
        [InlineData('A', 'a')]
        [InlineData('a', 'a')]
        [InlineData(' ', ' ')]
        public void ShouldTransformToLowercase(char value, char expected)
        {
            Assert.Equal(expected, value.ToLower());
        }

        [Theory]
        [InlineData('A', true)]
        [InlineData('a', true)]
        [InlineData(' ', false)]
        public void ShouldReturnIfIsAlphabet(char value, bool expected)
        {
            Assert.Equal(expected, value.IsAsciiAlphabetLetter());
        }
    }
}
