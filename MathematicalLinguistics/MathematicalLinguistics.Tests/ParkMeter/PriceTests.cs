using MathematicalLinguistics.ParkMeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace MathematicalLinguistics.Tests.ParkMeter
{
    public class PriceTests
    {
        [Fact]
        public void PriceFromGrosze()
        {
            var price = Price.FromGrosze(50);
            Assert.Equal(50, price.Grosze);
        }

        [Fact]
        public void PriceFromZlotys()
        {
            var price = Price.FromZlotys(5);
            Assert.Equal(500, price.Grosze);
        }

        [Fact]
        public void SubtractPrice()
        {
            var difference = Price.FromZlotys(113) - Price.FromZlotys(100);
            Assert.Equal(Price.FromZlotys(13).Grosze, difference.Grosze);
        }

        [Fact] 
        public void ToString()
        {
            var result = Price.FromZlotys(2).ToString();
            Assert.Equal("2zł", result);
        }

        [Fact]
        public void ToStringGrosze()
        {
            var result = Price.FromGrosze(50).ToString();
            Assert.Equal("50gr", result);
        }
    }
}
