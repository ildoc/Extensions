using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Xunit;

namespace Extensions.Tests
{
    public class DictionaryExtensionsTests
    {
        private readonly Dictionary<int, string> _dict = new() { { 1, "aaa" }, { 2, "bbb" }, { 3, "ccc" }, { 4, "ddd" } };

        [Fact]
        public void ShouldRunActionForEachItemWhileRemoving()
        {
            var str = "";

            _dict.RemoveAllWithAction(x => x.Key >= 3, x => str += x.Value);
            Assert.Equal(2, _dict.Count);
            Assert.Equal("cccddd", str);
        }

        [Fact]
        public void ShouldRemoveItemsWhere()
        {
            _dict.RemoveAll(x => x.Key >= 3);
            Assert.Equal(2, _dict.Count);
        }

        [Fact]
        public void ShouldTryGetValueIfExists()
        {
            Assert.Equal("bbb", _dict.GetValue(2));
        }

        [Fact]
        public void ShouldThrowIfValueDontExists()
        {
            var message = Assert.Throws<KeyNotFoundException>(() => _dict.GetValue(5)).Message;
            Assert.Equal("'5' not found in Dictionary", message);
        }

        [Fact]
        public void ShouldReturnDefaultIfValueDontExists()
        {
            Assert.Equal(default, _dict.GetValueOrDefault(5));
        }

        [Fact]
        public void ShouldSwapKeyValues()
        {
            var outputdict = new Dictionary<string, int> { { "aaa", 1 }, { "bbb", 2 }, { "ccc", 3 }, { "ddd", 4 } };
            Assert.Equal(outputdict, _dict.SwapKeyValue());
        }

        [Fact]
        public void ShouldRunActionForEachItem()
        {
            var str = "";
            _dict.Each(x => str += x.Value);
            Assert.Equal("aaabbbcccddd", str);
        }
    }
}
