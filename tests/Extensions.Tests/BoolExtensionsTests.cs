using Xunit;

namespace Extensions.Tests
{
    public class BoolExtensionsTests
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
    }
}
