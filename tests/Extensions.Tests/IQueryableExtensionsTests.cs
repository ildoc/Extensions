using System.Collections.Generic;
using System.Linq;
using Extensions.Enums;
using Xunit;

namespace Extensions.Tests
{
    public class IQueryableExtensionsTests
    {
        private class TestItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private readonly IQueryable<TestItem> _items = new List<TestItem> {
            new TestItem{ Id = 1, Name = "aaa"},
            new TestItem{ Id = 2, Name = "bbb"},
            new TestItem{ Id = 3, Name = "ccc"},
            new TestItem{ Id = 4, Name = "ddd"}
        }.AsQueryable();

        [Theory]
        [InlineData(SortingOptions.Ascending, "aaabbbcccddd")]
        [InlineData(SortingOptions.Descending, "dddcccbbbaaa")]
        public void ShouldOrderBy(SortingOptions opt, string expected)
        {
            Assert.Equal(expected, _items.OrderBy("Id", opt).Select(x => x.Name).Join(""));
        }
    }
}
