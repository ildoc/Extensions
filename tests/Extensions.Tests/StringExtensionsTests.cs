using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace Extensions.Tests
{
    public class StringExtensionsTests
    {
        public enum TestEnum
        {
            ValueA,
            ValueB
        }

        [Theory]
        [InlineData(TestEnum.ValueA, "ValueA")]
        [InlineData(default, default)]
        public void EnumStringShouldResolveToEnum(TestEnum expected, string value)
        {
            Assert.Equal(expected, value.ToEnum<TestEnum>());
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
        public void ShouldReturnAbsolutePathIfRelative()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Test");
            Assert.Equal(path, "Test".Absolutize());
        }

        [Fact]
        public void ShouldReturnAbsolutePathIfAlreadyAbsolute()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Test");
            Assert.Equal(path, path.Absolutize());
        }

        [Fact]
        public void AbsolutizeShouldThrowExceptionIfStringIsNull()
        {
            string test = default;
            var message = Assert.Throws<ArgumentException>(() => test.Absolutize()).Message;
            Assert.Equal("Cannot absolutize a null path", message);
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
            Assert.True(string.Empty.IsNullOrWhiteSpace());
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

        [Theory]
        [InlineData(default, 0)]
        [InlineData("42.1", 42.1)]
        [InlineData("asd", 0)]
        [InlineData("32,1", 32.1)]
        public void ShouldConvertToDouble(string input, double expected)
        {
            Assert.Equal(expected, input.ToDouble());
        }

        [Theory]
        [InlineData("provaprova", "oa", "prvprv")]
        public void ShouldRemoveCharFromString(string text1, string text2, string expected)
        {
            Assert.Equal(expected, text1.Except(text2));
        }

        [Theory]
        [InlineData("Test", "test")]
        [InlineData("TEST", "tEST")]
        [InlineData("T", "t")]
        [InlineData("t", "t")]
        [InlineData("", "")]
        [InlineData(default, default)]
        public void ShouldLowerFirstChar(string str, string expected)
        {
            Assert.Equal(expected, str.LowerFirstLetter());
        }

        [Theory]
        [InlineData("This is a test", 7, "This is…")]
        [InlineData("This is a test", 20, "This is a test")]
        public void ShouldTruncateAtWithEllipsis(string str, int charNum, string expected)
        {
            Assert.Equal(expected, str.TruncateAt(charNum, true));
        }

        [Theory]
        [InlineData("This is a test", 7, "This is")]
        [InlineData("This is a test", 20, "This is a test")]
        public void ShouldTruncateAtWithoutEllipsis(string str, int charNum, string expected)
        {
            Assert.Equal(expected, str.TruncateAt(charNum));
        }

        [Fact]
        public void ShouldThrowIfTruncateAtIsNegative()
        {
            var message = Assert.Throws<ArgumentOutOfRangeException>(() => "string".TruncateAt(-3)).Message;
            Assert.Equal("Length cannot be less than zero. (Parameter 'length')", message);
        }

        [Fact]
        public void ShouldTransformTemplate()
        {
            var o = new
            {
                Name = "Pino",
                Surname = "Cammino",
                Phrase = "Come una catapulta!"
            };
            var template = "{{Name}} {{Surname}} ha detto '{{Phrase}}'";
            var expected = "Pino Cammino ha detto 'Come una catapulta!'";

            Assert.Equal(expected, template.TransformTemplate(o));
        }

        [Theory]
        [InlineData("Test", "Test")]
        [InlineData("test", "Test")]
        [InlineData("t", "T")]
        [InlineData("T", "T")]
        [InlineData("multiple words", "Multiple words")]
        [InlineData("Multiple words", "Multiple words")]
        [InlineData("", "")]
        [InlineData(default, default)]
        public void ShouldCapitalizeFirstChar(string str, string expected)
        {
            Assert.Equal(expected, str.CapitalizeFirstLetter());
        }

        [Theory]
        [InlineData("Il pianista sull'oceano", "Il Pianista Sull'Oceano")]
        [InlineData("IL PIANISTA SULL'OCEANO", "Il Pianista Sull'Oceano")]
        [InlineData("il pianista sull'oceano", "Il Pianista Sull'Oceano")]
        [InlineData("titolo_con_underscore", "Titolo_Con_Underscore")]
        [InlineData("titolo-con-trattini", "Titolo-Con-Trattini")]
        [InlineData("titolo.con.punti", "Titolo.Con.Punti")]
        [InlineData("iL pIaNisTa sUlL'oCeAnO", "Il Pianista Sull'Oceano")]
        [InlineData("Il Pianista Sull'Oceano", "Il Pianista Sull'Oceano")]
        [InlineData("", "")]
        [InlineData(default, default)]
        public void ShouldCapitalizeEachFirstLetter(string str, string expected)
        {
            Assert.Equal(expected, str.ToTitleCase());
        }
    }
}
