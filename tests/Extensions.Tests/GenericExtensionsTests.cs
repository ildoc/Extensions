using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Extensions.Tests
{
    public class GenericExtensionsTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void BoolShouldResolveAsBool(bool value, bool expected)
        {
            value.ToBool().ShouldBe(expected);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(default, false)]
        [InlineData(7, true)]
        [InlineData(-7, true)]
        public void IntShouldResolveAsBool(int? value, bool expected)
        {
            value.ToBool().ShouldBe(expected);
        }

        [Theory]
        [InlineData("false", false)]
        [InlineData(default, false)]
        [InlineData("true", true)]
        [InlineData("asd", false)]
        public void StringShouldResolveAsBool(string value, bool expected)
        {
            value.ToBool().ShouldBe(expected);
        }

        [Theory]
        [InlineData(0f, false)]
        [InlineData(default, false)]
        [InlineData(7f, true)]
        [InlineData(-7f, true)]
        public void FloatShouldResolveAsBool(float? value, bool expected)
        {
            value.ToBool().ShouldBe(expected);
        }

        [Theory]
        [InlineData(0.0, false)]
        [InlineData(default, false)]
        [InlineData(7.0, true)]
        [InlineData(-7.0, true)]
        public void DoubleShouldResolveAsBool(double? value, bool expected)
        {
            value.ToBool().ShouldBe(expected);
        }

        [Fact]
        public void ObjectShouldResolveAsTrue()
        {
            new { Id = 7 }.ToBool().ShouldBeTrue();
        }

        [Fact]
        public void NullShouldResolveAsFlase()
        {
            object value = null;

            value.ToBool().ShouldBeFalse();
        }

        [Theory]
        [InlineData('b', "abcd", true)]
        [InlineData('f', "abcd", false)]
        public void ShouldCheckIfValueIsInString(char value, string fullString, bool expected)
        {
            value.IsIn(fullString).ShouldBe(expected);
        }

        [Theory]
        [InlineData('b', "abcd", false)]
        [InlineData('f', "abcd", true)]
        public void ShouldCheckIfValueIsNotInString(char value, string fullString, bool expected)
        {
            value.IsNotIn(fullString).ShouldBe(expected);
        }

        [Fact]
        public void ShouldCheckIfStringIsInStrings()
        {
            var value = "test";
            var strings = new List<string> { "this", "is", "a", "test" };

            value.IsIn(strings).ShouldBeTrue();
            value.IsNotIn(strings).ShouldBeFalse();
        }

        [Fact]
        public void ShouldCheckIfStringIsNotInStrings()
        {
            var value = "not";
            var strings = new List<string> { "this", "is", "a", "test" };

            value.IsIn(strings).ShouldBeFalse();
            value.IsNotIn(strings).ShouldBeTrue();
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

            casted.Id.ShouldBe(anon.Id);
            casted.Description.ShouldBe(anon.Description);
            casted.TimeStamp.ShouldBe(anon.TimeStamp);
        }

        [Fact]
        public void ShouldClone()
        {
            var a = new TestClass
            {
                Id = 7,
                Description = "test",
                TimeStamp = DateTime.Now
            };

            var b = a.Clone();

            b.ShouldBeEquivalentTo(a);
        }

        [Theory]
        [InlineData(null, null, true)]
        [InlineData(3, "asd", false)]
        [InlineData(null, 3, false)]
        [InlineData(3, null, false)]
        [InlineData("asd", "asd", true)]
        public void ShouldReturnEqual(object a, object b, bool expected)
        {
            a.IsEqualTo(b).ShouldBe(expected);
        }

        private class TestClass
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}
