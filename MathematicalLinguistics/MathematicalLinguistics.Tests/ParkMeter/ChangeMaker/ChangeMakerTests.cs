using MathematicalLinguistics.ParkMeter;
using MathematicalLinguistics.ParkMeter.ChangeMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MathematicalLinguistics.Tests.ParkMeter
{
    public class ChangeMakerTests
    {
        private ChangeMaker _changeMaker;

        [Fact]
        public void NoChange()
        {
            _changeMaker = MakeChangeMaker(MakeCoinsStorage1());
            var expected = new List<Coin>();
            TestChange(Price.FromZlotys(10), Price.FromZlotys(10), expected);
        }

        [Fact]
        public void Change()
        {
            _changeMaker = MakeChangeMaker(MakeCoinsStorage1());
            var price = Price.FromZlotys(100);
            var actual = Price.FromZlotys(113);
            var expected = new List<Coin>
            {
                //change is 13 zlotys
                Coin.FromZlotys(5), // 13 - 5 = 8
                Coin.FromZlotys(5), // 8 - 5 = 3
                Coin.FromZlotys(2), // 3 - 2 = 1
                Coin.FromZlotys(1)  // 1 - 1 = 0
            };
            TestChange(price, actual, expected);
        }

        private CoinStorage MakeCoinsStorage1()
        {
            var storage = new CoinStorage();
            storage.Insert(Coin.FromZlotys(1), 100)
                   .Insert(Coin.FromZlotys(2), 100)
                   .Insert(Coin.FromZlotys(5), 100);
            return storage;
        }

        [Fact]
        public void ChangeWhenSomeCoinAreMissingInCoinsStorage()
        {
            _changeMaker = MakeChangeMaker(MakeCoinsStorage2());
            var price = Price.FromZlotys(100);
            var actual = Price.FromZlotys(113);
            var expected = new List<Coin>
            {
                // change is 13 zlotys
                Coin.FromZlotys(5), // 13 - 5 = 8 
                Coin.FromZlotys(2), // 8 - 2 = 6
                Coin.FromZlotys(2), // 6 - 2 = 4
                Coin.FromZlotys(2), // 4 - 2 = 2
                Coin.FromZlotys(2)  // 2 - 2 = 0
            };

            TestChange(price, actual, expected);
        }

        private CoinStorage MakeCoinsStorage2()
        {
            var storage = new CoinStorage();
            storage.Insert(Coin.FromZlotys(1), 100)
                   .Insert(Coin.FromZlotys(2), 100)
                   .Insert(Coin.FromZlotys(5), 1);
            return storage;
        }

        private ChangeMaker MakeChangeMaker(CoinStorage storage)
        {
            var _changeMaker = new ChangeMaker(storage);
            return _changeMaker;
        }

        private void TestChange(Price price, Price actual, List<Coin> expected)
        {
            var result = _changeMaker.Make(price, actual);

            Assert.Equal(expected.Count, result.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.Equal(expected[i].Grosze, result[i].Grosze);
            }
        }
    }
}
