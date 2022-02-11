using System;
using Xunit;

namespace Utils.Tests
{
    public class FiscalCodeTests
    {
        [Fact]
        public void ShouldCheckFiscalCode()
        {
            var result = FiscalCode.EasyCheck("gmifpp90h22d969j", "filippo", "giomi", new DateTime(1990, 6, 22));
            Assert.True(result);
        }

        [Fact]
        public void ShouldCheckFiscalCodeSurname()
        {
            var result = FiscalCode.CheckSurname("gmi", "giomi");
            Assert.True(result);
        }
    }
}
