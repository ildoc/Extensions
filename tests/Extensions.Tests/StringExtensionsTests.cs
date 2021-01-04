using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace Extensions.Tests
{
    public class StringExtensionsTests
    {
        private enum TestEnum
        {
            ValueA,
            ValueB
        }

        [Fact]
        public void EnumStringShouldResolveToEnum()
        {
            Assert.Equal(TestEnum.ValueA, "ValueA".ToEnum<TestEnum>());
        }

        [Fact]
        public void UnrecognizedEnumStringShouldThrowException()
        {
            var message = Assert.Throws<ArgumentException>(() => "ValueC".ToEnum<TestEnum>()).Message;
            Assert.Equal("Cannot parse ValueC to TestEnum", message);
        }

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "TEST", true)]
        [InlineData("test", "notTest", false)]
        [InlineData("test", default, false)]
        [InlineData(default, "test", false)]
        [InlineData(default, default, true)]
        public void ShouldReturnIfEqualCaseInsensitive(string value1, string value2, bool expected)
        {
            Assert.Equal(expected, value1.IsEqualTo(value2));
        }

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "TEST", true)]
        [InlineData("test", "notTest", false)]
        [InlineData("test", default, false)]
        [InlineData(default, "test", false)]
        [InlineData(default, default, true)]
        public void ShouldReturnIfEqualCaseInsensitiveWithParam(string value1, string value2, bool expected)
        {
            Assert.Equal(expected, value1.IsEqualTo(value2, caseSensitive: false));
        }

        [Theory]
        [InlineData("test", "test", true)]
        [InlineData("test", "TEST", false)]
        [InlineData("test", "notTest", false)]
        [InlineData("test", default, false)]
        [InlineData(default, "test", false)]
        [InlineData(default, default, true)]
        public void ShouldReturnIfEqualCaseSensitive(string value1, string value2, bool expected)
        {
            Assert.Equal(expected, value1.IsEqualTo(value2, caseSensitive: true));
        }

        [Fact]
        public void ShouldReturnAbsolutePath()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Test");
            Assert.Equal(path, "Test".Absolutize());
        }
        [Fact]
        public void AbsolutizeShouldThrowExceptionIfStringIsNull()
        {
            string test = default;
            var message = Assert.Throws<ArgumentException>(() => test.Absolutize()).Message;
            Assert.Equal("Cannot absolutize a null path", message);
        }

        [Theory]
        [InlineData("test string", 2, 7, "st st")]
        [InlineData("test string", 2, -2, "st stri")]
        public void ShouldSliceString(string value, int start, int end, string expected)
        {
            Assert.Equal(expected, value.Slice(start, end));
        }

        [Fact]
        public void SliceShouldThrowExceptionIfStartIsNegative()
        {
            var message = Assert.Throws<ArgumentException>(() => "test string".Slice(-1, 5)).Message;
            Assert.Equal("Slice cannot have a negative start", message);
        }

        [Fact]
        public void SliceShouldThrowExceptionIfStringIsNull()
        {
            string test = default;
            var message = Assert.Throws<ArgumentException>(() => test.Slice(2, 5)).Message;
            Assert.Equal("Cannot slice a null string", message);
        }

        [Theory]
        [InlineData("ThisIsATestString", "This Is A Test String")]
        [InlineData("_ThisIsATestString", "This Is A Test String")]
        [InlineData("alllowercase", "alllowercase")]
        [InlineData("ALLUPPERCASE", "ALLUPPERCASE")]
        [InlineData("MIxeDCasE", "M Ixe D Cas E")]
        public void ShouldDivideStringBasedOnCamelCase(string value, string expected)
        {
            Assert.Equal(expected, value.FromCamelCase());
        }

        [Fact]
        public void FromCamelCaseShouldThrowExceptionIfStartIsNegative()
        {
            string test = default;
            var message = Assert.Throws<ArgumentException>(() => test.FromCamelCase()).Message;
            Assert.Equal("Cannot perform FromCamelCase on a null string", message);
        }

        [Theory]
        [InlineData(default, "replaced string", "replaced string")]
        [InlineData("actual string", "replaced string", "replaced string")]
        [InlineData("actual string", null, "actual string")]
        [InlineData(default, default, default)]
        public void ShouldReplaceStringIfReplaceIsNotNull(string value, string replace, string expected)
        {
            Assert.Equal(expected, value.ReplaceWith(replace));
        }

        [Theory]
        [InlineData(default, true)]
        [InlineData("string", false)]
        [InlineData("", true)]
        public void ShouldCheckIfStringIsNullOrEmpty(string value, bool expected)
        {
            Assert.Equal(expected, value.IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmptyShouldReturnTrueWithStringEmpty()
        {
            Assert.True(string.Empty.IsNullOrEmpty());
        }

        [Theory]
        [InlineData(default, true)]
        [InlineData("string", false)]
        [InlineData(" ", true)]
        [InlineData("", true)]
        public void ShouldCheckIfStringIsNullOrWhiteSpace(string value, bool expected)
        {
            Assert.Equal(expected, value.IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpaceShouldReturnTrueWithStringEmpty()
        {
            Assert.True(string.Empty.IsNullOrEmpty());
        }

        [Theory]
        [InlineData(default, false)]
        [InlineData("string", false)]
        [InlineData(" ", false)]
        [InlineData("3", true)]
        [InlineData("+3", true)]
        [InlineData(" 3", true)]
        [InlineData(" 3 ", true)]
        [InlineData("3 ", true)]
        [InlineData("almost 3", false)]
        [InlineData("3,3", false)]
        [InlineData("3.3", false)]
        [InlineData("+3.3", false)]
        [InlineData("- 3.3", false)]
        [InlineData(" - 3.3 ", false)]
        public void ShouldCheckIfStringIsNumeric(string value, bool expected)
        {
            Assert.Equal(expected, value.IsNumeric());
        }

        [Fact]
        public void ShouldSplitString()
        {
            var source = "splitted";
            var expected = new List<string> { "s", "p", "l", "i", "t", "t", "e", "d" };
            Assert.Equal(expected, source.SplitString());
        }

        [Fact]
        public void SplitStringShouldThrowExceptionIfStringIsNull()
        {
            string test = default;
            var message = Assert.Throws<ArgumentException>(() => test.SplitString()).Message;
            Assert.Equal("Cannot split a null string", message);
        }

        [Theory]
        [InlineData(default, 1, default)]
        [InlineData("string", 0, "string")]
        [InlineData("string", 2, "ring")]
        [InlineData("string", 15, "string")]
        public void ShouldThrowNoErrorsOnTolerantSubstring(string value, int startIndex, string expected)
        {
            Assert.Equal(expected, value.TolerantSubstring(startIndex));
        }

        [Theory]
        [InlineData(default, 1, 3, default)]
        [InlineData("string", 0, 2, "st")]
        [InlineData("string", 2, 0, "")]
        [InlineData("string", 2, 2, "ri")]
        [InlineData("string", 15, 23, "string")]
        [InlineData("string", 2, 15, "ring")]
        public void ShouldThrowNoErrorsOnTolerantSubstringWithLength(string value, int startIndex, int length, string expected)
        {
            Assert.Equal(expected, value.TolerantSubstring(startIndex, length));
        }

        [Fact]
        public void TolerantSubstringShouldThrowExceptionIfIndexIsNegative()
        {
            var message = Assert.Throws<ArgumentOutOfRangeException>(() => "string".TolerantSubstring(-3)).Message;
            Assert.Equal("StartIndex cannot be less than zero. (Parameter 'startIndex')", message);
        }

        [Fact]
        public void TolerantSubstringShouldThrowExceptionIfLengthIsNegative()
        {
            var message = Assert.Throws<ArgumentOutOfRangeException>(() => "string".TolerantSubstring(0, -3)).Message;
            Assert.Equal("Length cannot be less than zero. (Parameter 'length')", message);
        }

        [Theory]
        [InlineData(default, default)]
        [InlineData("test diacritics èòàùçé", "test diacritics eoauce")]
        public void ShouldRemoveDiacritics(string input, string expected)
        {
            Assert.Equal(expected, input.RemoveDiacritics());
        }

        [Theory]
        [InlineData(default, 0)]
        [InlineData("42", 42)]
        [InlineData("asd", 0)]
        [InlineData("32.1", 0)]
        public void ShouldConvertToInt32(string input, int expected)
        {
            Assert.Equal(expected, input.ToInt32());
        }
    }
}
