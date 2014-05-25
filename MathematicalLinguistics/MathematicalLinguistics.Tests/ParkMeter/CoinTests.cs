using MathematicalLinguistics.ParkMeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace MathematicalLinguistics.Tests
{
    public class CoinTests
    {
        [Fact]
        public void FromGrosze()
        {
            var grosz = Coin.FromGrosze(1);
            Assert.Equal(1, grosz.Grosze);
        }

        [Fact]
        public void FromZlotys()
        {
            var zloty = Coin.FromZlotys(1);
            Assert.Equal(100, zloty.Grosze);
        }

        [Theory]
        [InlineData("1zł", 100)]
        [InlineData("50gr", 50)]
        public void Parse(string value, int grosze)
        {
            Assert.Equal(grosze, Coin.Parse(value).Grosze);
        }
    }
}
