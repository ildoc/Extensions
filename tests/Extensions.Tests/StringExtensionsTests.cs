using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Shouldly;
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
            value.ToEnum<TestEnum>().ShouldBe(expected);
        }

        [Fact]
        public void UnrecognizedEnumStringShouldThrowException()
        {
            var message = Should.Throw<ArgumentException>(() => "ValueC".ToEnum<TestEnum>()).Message;
            message.ShouldBe("Cannot parse ValueC to TestEnum");
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
            value1.IsEqualTo(value2).ShouldBe(expected);
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
            value1.IsEqualTo(value2, caseSensitive: false).ShouldBe(expected);
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
            value1.IsEqualTo(value2, caseSensitive: true).ShouldBe(expected);
        }

        [Fact]
        public void ShouldReturnAbsolutePathIfRelative()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Test");
            "Test".Absolutize().ShouldBe(path);
        }

        [Fact]
        public void ShouldReturnAbsolutePathIfAlreadyAbsolute()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Test");
            path.Absolutize().ShouldBe(path);
        }

        [Fact]
        public void AbsolutizeShouldThrowExceptionIfStringIsNull()
        {
            string test = default;
            var message = Should.Throw<ArgumentException>(() => test.Absolutize()).Message;
            message.ShouldBe("Cannot absolutize a null path");
        }

        [Theory]
        [InlineData("ThisIsATestString", "This Is A Test String")]
        [InlineData("_ThisIsATestString", "This Is A Test String")]
        [InlineData("alllowercase", "alllowercase")]
        [InlineData("ALLUPPERCASE", "ALLUPPERCASE")]
        [InlineData("MIxeDCasE", "M Ixe D Cas E")]
        public void ShouldDivideStringBasedOnCamelCase(string value, string expected)
        {
            value.FromCamelCase().ShouldBe(expected);
        }

        [Fact]
        public void FromCamelCaseShouldThrowExceptionIfStartIsNegative()
        {
            string test = default;
            var message = Should.Throw<ArgumentException>(() => test.FromCamelCase()).Message;
            message.ShouldBe("Cannot perform FromCamelCase on a null string");
        }

        [Theory]
        [InlineData(default, "replaced string", "replaced string")]
        [InlineData("actual string", "replaced string", "replaced string")]
        [InlineData("actual string", null, "actual string")]
        [InlineData(default, default, default)]
        public void ShouldReplaceStringIfReplaceIsNotNull(string value, string replace, string expected)
        {
            value.ReplaceWith(replace).ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, true)]
        [InlineData("string", false)]
        [InlineData("", true)]
        public void ShouldCheckIfStringIsNullOrEmpty(string value, bool expected)
        {
            value.IsNullOrEmpty().ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, true)]
        [InlineData("string", false)]
        [InlineData(" ", true)]
        [InlineData("", true)]
        public void ShouldCheckIfStringIsNullOrWhiteSpace(string value, bool expected)
        {
            value.IsNullOrWhiteSpace().ShouldBe(expected);
        }

        [Fact]
        public void IsNullOrWhiteSpaceShouldReturnTrueWithStringEmpty()
        {
            string.Empty.IsNullOrWhiteSpace().ShouldBeTrue();
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
            value.IsNumeric().ShouldBe(expected);
        }

        [Fact]
        public void ShouldSplitString()
        {
            var source = "splitted";
            var expected = new List<string> { "s", "p", "l", "i", "t", "t", "e", "d" };
            source.SplitString().ShouldBe(expected);
        }

        [Fact]
        public void SplitStringShouldThrowExceptionIfStringIsNull()
        {
            string test = default;
            var message = Should.Throw<ArgumentException>(() => test.SplitString()).Message;
            message.ShouldBe("Cannot split a null string");
        }

        [Theory]
        [InlineData(default, 1, default)]
        [InlineData("string", 0, "string")]
        [InlineData("string", 2, "ring")]
        [InlineData("string", 15, "string")]
        public void ShouldThrowNoErrorsOnTolerantSubstring(string value, int startIndex, string expected)
        {
            value.TolerantSubstring(startIndex).ShouldBe(expected);
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
            value.TolerantSubstring(startIndex, length).ShouldBe(expected);
        }

        [Fact]
        public void TolerantSubstringShouldThrowExceptionIfIndexIsNegative()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => "string".TolerantSubstring(-3));
        }

        [Fact]
        public void TolerantSubstringShouldThrowExceptionIfLengthIsNegative()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => "string".TolerantSubstring(0, -3));
        }

        [Theory]
        [InlineData(default, default)]
        [InlineData("test diacritics èòàùçé", "test diacritics eoauce")]
        public void ShouldRemoveDiacritics(string input, string expected)
        {
            input.RemoveDiacritics().ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, 0)]
        [InlineData("42", 42)]
        [InlineData("asd", 0)]
        [InlineData("32.1", 0)]
        public void ShouldConvertToInt32(string input, int expected)
        {
            input.ToInt32().ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, 0)]
        [InlineData("42.1", 42.1)]
        [InlineData("asd", 0)]
        [InlineData("32,1", 32.1)]
        public void ShouldConvertToDouble(string input, double expected)
        {
            input.ToDouble().ShouldBe(expected);
        }

        [Theory]
        [InlineData("provaprova", "oa", "prvprv")]
        public void ShouldRemoveCharFromString(string text1, string text2, string expected)
        {
            text1.Except(text2).ShouldBe(expected);
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
            str.LowerFirstLetter().ShouldBe(expected);
        }

        [Theory]
        [InlineData("This is a test", 7, "This is…")]
        [InlineData("This is a test", 20, "This is a test")]
        public void ShouldTruncateAtWithEllipsis(string str, int charNum, string expected)
        {
            str.TruncateAt(charNum, true).ShouldBe(expected);
        }

        [Theory]
        [InlineData("This is a test", 7, "This is")]
        [InlineData("This is a test", 20, "This is a test")]
        public void ShouldTruncateAtWithoutEllipsis(string str, int charNum, string expected)
        {
            str.TruncateAt(charNum).ShouldBe(expected);
        }

        [Fact]
        public void ShouldThrowIfTruncateAtIsNegative()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => "string".TruncateAt(-3));
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

            template.TransformTemplate(o).ShouldBe(expected);
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
            str.CapitalizeFirstLetter().ShouldBe(expected);
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
            str.ToTitleCase().ShouldBe(expected);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("c", "c")]
        [InlineData("ciao", "oaic")]
        [InlineData(default, default)]
        public void ShouldReverseString(string str, string expected)
        {
            str.Reverse().ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, 3, default)]
        [InlineData("", 3, "")]
        [InlineData("#", 3, "###")]
        [InlineData("-", 5, "-----")]
        public void ShouldRepeatString(string str, int times, string expected)
        {
            str.Repeat(times).ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, "_", default)]
        [InlineData("noprefix", "_", "_noprefix")]
        [InlineData("_withprefix", "_", "_withprefix")]
        public void ShouldEnsureStringStartsWith(string str, string value, string expected)
        {
            str.EnsureStartsWith(value).ShouldBe(expected);
        }

        [Theory]
        [InlineData(default, "/", default)]
        [InlineData("https://ildoc.dev", "/", "https://ildoc.dev/")]
        [InlineData("https://ildoc.dev/", "/", "https://ildoc.dev/")]
        public void ShouldEnsureStringEndsWith(string str, string value, string expected)
        {
            str.EnsureEndsWith(value).ShouldBe(expected);
        }
    }
}
