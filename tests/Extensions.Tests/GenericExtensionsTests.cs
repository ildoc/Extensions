using System;
using System.Collections.Generic;
using Xunit;

namespace Extensions.Tests
{
    public class GenericExtensionsTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void BoolShouldResolveAsBool(bool value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.ToBool());
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(default, false)]
        [InlineData(7, true)]
        [InlineData(-7, true)]
        public void IntShouldResolveAsBool(int? value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.ToBool());
        }

        [Theory]
        [InlineData("false", false)]
        [InlineData(default, false)]
        [InlineData("true", true)]
        [InlineData("asd", false)]
        public void StringShouldResolveAsBool(string value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.ToBool());
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(default, false)]
        [InlineData(7f, true)]
        [InlineData(-7f, true)]
        public void FloatShouldResolveAsBool(float? value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.ToBool());
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(default, false)]
        [InlineData(7.0, true)]
        [InlineData(-7.0, true)]
        public void DoubleShouldResolveAsBool(double? value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.ToBool());
        }

        [Fact]
        public void ObjectShouldResolveAsTrue()
        {
            Assert.True(new { Id = 7 }.ToBool());
        }

        [Fact]
        public void NullShouldResolveAsFlase()
        {
            object value = null;

            Assert.False(value.ToBool());
        }

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

        [Fact]
        public void ShouldCheckIfStringIsInStrings()
        {
            var value = "test";
            var strings = new List<string> { "this", "is", "a", "test" };

            Assert.True(value.IsIn(strings));
            Assert.False(value.IsNotIn(strings));
        }

        [Fact]
        public void ShouldCheckIfStringIsNotInStrings()
        {
            var value = "not";
            var strings = new List<string> { "this", "is", "a", "test" };

            Assert.False(value.IsIn(strings));
            Assert.True(value.IsNotIn(strings));
        }

        [Fact]
        public void ShouldCastFromAnonymousObject()
        {
            var anon = new
            {
                Id = 7,
                Description = "test",
                TimeStamp = DateTime.Now
            };

            var casted = anon.AnonymousCastTo<TestClass>();

            Assert.Equal(casted.Id, anon.Id);
            Assert.Equal(casted.Description, anon.Description);
            Assert.Equal(casted.TimeStamp, anon.TimeStamp);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(3, "asd", false)]
        [InlineData(null, 3, false)]
        [InlineData(3, null, false)]
        [InlineData("asd", "asd", true)]
        public void ShouldReturnEqual(object a, object b, bool expected)
        {
            Assert.Equal(expected, a.IsEqualTo(b));
        }

        private class TestClass
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}
