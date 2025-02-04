﻿using System.Collections.Generic;
using Shouldly;
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


            list.ShouldContain("Fizz");
            list.ShouldNotContain("Buzz");
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
            list.Count.ShouldBe(2);
        }
    }
}
