using System.ComponentModel;
using Shouldly;
using Xunit;

namespace Extensions.Tests
{
    public class EnumExtensionsTests
    {
        public enum TestEnum
        {
            [Description("ValueA Description")]
            ValueA,
            ValueB,
            ValueC,
            ValueD,
        }

        [Theory]
        [InlineData(TestEnum.ValueA, "ValueA Description")]
        [InlineData(TestEnum.ValueB, "ValueB")]
        public void ShouldGetDescription(TestEnum testEnumValue, string expected)
        {
            testEnumValue.GetDescription().ShouldBe(expected);
        }
    }
}
