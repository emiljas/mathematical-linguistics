using MathematicalLinguistics.ParkMeter;
using MathematicalLinguistics.ParkMeter.ChangeMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MathematicalLinguistics.Tests
{
    public class CoinStorageTests
    {
        private CoinStorage _storage = new CoinStorage();

        public CoinStorageTests()
        {
            _storage.Insert(Coin.FromZlotys(2), 54);
            _storage.Insert(Coin.FromGrosze(10), 3);
            _storage.Insert(Coin.FromZlotys(5), 8);
        }

        [Fact]
        public void GroupsAreOrderByGrosze()
        {
            Assert.Equal(Coin.FromZlotys(5).Grosze, _storage.CoinsGroups[0].Coin.Grosze);
            Assert.Equal(Coin.FromZlotys(2).Grosze, _storage.CoinsGroups[1].Coin.Grosze);
            Assert.Equal(Coin.FromGrosze(10).Grosze, _storage.CoinsGroups[2].Coin.Grosze);
        }

        [Fact]
        public void ToString()
        {
            var expected = "5zł x 8\t2zł x 54\t10gr x 3";
            var result = _storage.ToString();
            Assert.Equal(expected, result);
        }
    }
}
