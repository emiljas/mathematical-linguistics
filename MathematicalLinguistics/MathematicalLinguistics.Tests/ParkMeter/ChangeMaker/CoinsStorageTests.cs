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

        [Fact]
        public void GroupsAreOrderByGrosze()
        {
            _storage.Insert(Coin.FromZlotys(2), 54);
            _storage.Insert(Coin.FromGrosze(10), 3);
            _storage.Insert(Coin.FromZlotys(5), 8);

            Assert.Equal(Coin.FromZlotys(5).Grosze, _storage.CoinsGroups[0].Coin.Grosze);
            Assert.Equal(Coin.FromZlotys(2).Grosze, _storage.CoinsGroups[1].Coin.Grosze);
            Assert.Equal(Coin.FromGrosze(10).Grosze, _storage.CoinsGroups[2].Coin.Grosze);
            
        }
    }
}
