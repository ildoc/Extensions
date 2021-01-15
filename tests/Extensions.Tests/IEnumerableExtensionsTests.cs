using System.Collections.Generic;
using Xunit;

namespace Extensions.Tests
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void ShouldJoinListString()
        {
            var list = new List<string> { "questa", "è", "una", "lista" };
            Assert.Equal("questa, è, una, lista", list.Join(", "));
        }

        [Fact]
        public void ShouldJoinListChar()
        {
            var list = new List<string> { "questa", "è", "una", "lista" };
            Assert.Equal("questa,è,una,lista", list.Join(','));
        }
    }
}
