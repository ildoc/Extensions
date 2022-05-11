using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Extensions.Tests
{
    public class ICollectionExtensionsTests
    {
        [Fact]
        public void AddIf()
        {
            var list = new List<string>();

            list.AddIf(_ => true, "Fizz"); // Add "Fizz" value
            list.AddIf(_ => false, "Buzz"); // Doesn't add "Buzz" value


            list.Should().Contain("Fizz");
            list.Should().NotContain("Buzz");
        }

        [Fact]
        public void AddIfNotContains()
        {
            // Type
            var list = new List<string> { "FizzExisting" };

            // Examples
            list.AddIfNotContains("Fizz"); // Add "Fizz" value
            list.AddIfNotContains("FizzExisting"); // Doesn't add "FizzExisting" value, the Collection already contains it.

            // Unit Test
            list.Count.Should().Be(2);
        }
    }
}
