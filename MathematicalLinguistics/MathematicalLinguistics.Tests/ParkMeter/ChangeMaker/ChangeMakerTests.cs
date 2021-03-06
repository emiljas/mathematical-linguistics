﻿using MathematicalLinguistics.ParkMeter;
using MathematicalLinguistics.ParkMeter.Change;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public void MakeChange()
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
        public void MakeChange_WhenSomeCoinAreMissingInCoinsStorage()
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

        [Fact]
        public void MakeChange_WhenMissingCoinInCoinStorage_ThrowsMissingCoinsInCoinStorageException()
        {
            _changeMaker = MakeChangeMaker(MakeCoinsStorage3());
            var price = Price.FromZlotys(4);
            var actual = Price.FromZlotys(8);
            
            Assert.ThrowsDelegate make = () => _changeMaker.Make(price, actual);

            Assert.Throws<MissingCoinsInCoinStorageException>(make);
        }

        private CoinStorage MakeCoinsStorage3()
        {
            var storage = new CoinStorage();
            storage.Insert(Coin.FromZlotys(1), 1)
                   .Insert(Coin.FromZlotys(2), 1);
            return storage;
        }

        [Fact]
        public void MakeChange_WhenCoinStorageIsEmpty_ThrowsMissingCoinsInCoinStorageException()
        {
            _changeMaker = new ChangeMaker(new CoinStorage());
            var price = Price.FromZlotys(4);
            var actual = Price.FromZlotys(8);

            Assert.ThrowsDelegate make = () => _changeMaker.Make(price, actual);

            Assert.Throws<MissingCoinsInCoinStorageException>(make);
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

        [Fact]
        public void StorageNotChangeWithoutCommit()
        {
            var storage = new CoinStorage();
            storage.Insert(Coin.FromGrosze(1), 1);

            var changeMaker = new ChangeMaker(storage);
            changeMaker.Make(Price.FromGrosze(1), Price.FromGrosze(2));

            Assert.Equal(1, storage.CoinsGroups.Count);
            Assert.Equal(1, storage.CoinsGroups[0].Count);
        }

        [Fact]
        public void StorageChangeAfterCommit()
        {
            var storage = new CoinStorage();
            storage.Insert(Coin.FromGrosze(1), 1);

            var changeMaker = new ChangeMaker(storage);
            changeMaker.Make(Price.FromGrosze(1), Price.FromGrosze(2));

            changeMaker.Commit();

            var commitedStorage = changeMaker.CoinStorage;

            Assert.Equal(1, commitedStorage.CoinsGroups.Count);
            Assert.Equal(0, commitedStorage.CoinsGroups[0].Count);
        }
    }
}
