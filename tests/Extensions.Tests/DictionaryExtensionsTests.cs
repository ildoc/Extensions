using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Shouldly;
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

            _dict.Count.ShouldBe(2);
            str.ShouldBe("cccddd");
        }

        [Fact]
        public void ShouldRemoveItemsWhere()
        {
            _dict.RemoveAll(x => x.Key >= 3);
            _dict.Count.ShouldBe(2);
        }

        [Fact]
        public void ShouldTryGetValueIfExists()
        {
            _dict.GetValue(2).ShouldBe("bbb");
        }

        [Fact]
        public void ShouldThrowIfValueDontExists()
        {
            var message = Should.Throw<KeyNotFoundException>(() => _dict.GetValue(5)).Message;
            message.ShouldBe("'5' not found in Dictionary");
        }

        [Fact]
        public void ShouldReturnDefaultIfValueDontExists()
        {
            _dict.GetValueOrDefault(4).ShouldBe("ddd");
            _dict.GetValueOrDefault(5).ShouldBe(default);
        }

        [Fact]
        public void ShouldSwapKeyValues()
        {
            var outputdict = new Dictionary<string, int> { { "aaa", 1 }, { "bbb", 2 }, { "ccc", 3 }, { "ddd", 4 } };
            _dict.SwapKeyValue().ShouldBeEquivalentTo(outputdict);
        }

        [Fact]
        public void ShouldRunActionForEachItem()
        {
            var str = "";
            _dict.Each(x => str += x.Value);

            str.ShouldBe("aaabbbcccddd");
        }

        [Fact]
        public void ShouldConvertToDictionary()
        {
            IEnumerable<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>> {
                new KeyValuePair<int, string>(1,"aaa"),
                new KeyValuePair<int, string>(2,"bbb"),
                new KeyValuePair<int, string>(3,"ccc"),
                new KeyValuePair<int, string>(4,"ddd"),
            };

            list.ToDictionary().ShouldBeEquivalentTo(_dict);
        }
    }
}
