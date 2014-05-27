using MathematicalLinguistics.ParkMeter;
using MathematicalLinguistics.ParkMeter.Change;
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
            _storage.Insert(Coin.FromZlotys(2), 54)
                    .Insert(Coin.FromGrosze(10), 3)
                    .Insert(Coin.FromZlotys(5), 8);
        }

        [Fact]
        public void Remove()
        {
            _storage.Remove(new List<Coin>{ Coin.FromZlotys(5), Coin.FromZlotys(5) });
            var group5zl = _storage.CoinsGroups.Single(c => c.Coin.Grosze == 500);

            Assert.Equal(6, group5zl.Count);
        }

        [Fact]
        public void GroupsAreOrderByGrosze()
        {
            Assert.Equal(Coin.FromZlotys(5).Grosze, _storage.CoinsGroups[0].Coin.Grosze);
            Assert.Equal(Coin.FromZlotys(2).Grosze, _storage.CoinsGroups[1].Coin.Grosze);
            Assert.Equal(Coin.FromGrosze(10).Grosze, _storage.CoinsGroups[2].Coin.Grosze);
        }

        [Fact]
        public void GroupAreConnected()
        {
            _storage.Insert(Coin.FromZlotys(5), 2);
            var group5zł = _storage.CoinsGroups.Single(c => c.Coin.Grosze == 500);
            Assert.Equal(10, group5zł.Count);
        }

        [Fact]
        public void ToString()
        {
            var expected = "5zł x 8\t2zł x 54\t10gr x 3";
            var result = _storage.ToString();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Clone()
        {
            var copy = _storage.Clone();

            copy.Insert(Coin.FromGrosze(1), 1);

            Assert.Equal(_storage.CoinsGroups.Count, 3);
            Assert.Equal(copy.CoinsGroups.Count, 4);
        }
    }
}
