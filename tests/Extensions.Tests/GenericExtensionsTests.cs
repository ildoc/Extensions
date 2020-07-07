using System;
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

        private class TestClass
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}
