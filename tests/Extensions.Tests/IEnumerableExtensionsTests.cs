using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Extensions.Tests
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void ShouldJoinListString()
        {
            var list = new List<string> { "questa", "è", "una", "lista" };
            list.Join(", ").ShouldBe("questa, è, una, lista");
        }

        [Fact]
        public void ShouldJoinListChar()
        {
            var list = new List<string> { "questa", "è", "una", "lista" };
            list.Join(',').ShouldBe("questa,è,una,lista");
        }
    }
}
