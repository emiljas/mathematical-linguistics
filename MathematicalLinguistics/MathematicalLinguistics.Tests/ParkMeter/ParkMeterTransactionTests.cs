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
    public class ParkMeterTransactionTests
    {
        private ParkMeterTransaction _parkMeter;

        public ParkMeterTransactionTests()
        {
            var coinStorage = new CoinStorage();
            coinStorage.Insert(Coin.FromZlotys(1), 10)
                       .Insert(Coin.FromZlotys(2), 7)
                       .Insert(Coin.FromZlotys(5), 2);
            var changeMaker = new ChangeMaker(coinStorage);

            _parkMeter = new ParkMeterTransaction(changeMaker);
        }

        [Fact]
        public void WaitingForMoreCoinsIfNoCoinInserted()
        {
            Assert.Equal(ParkMeterState.WaitingForMoreCoins, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_CoinSumEquals7zł_AcceptingState()
        {
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(Coin.FromZlotys(5));
            _parkMeter.InsertCoin(Coin.FromZlotys(1));

            Assert.Equal(ParkMeterState.AcceptingState, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_CoinSumEquals7złWithWrongCoin_AcceptingState()
        {
            var wrongCoin = Coin.FromGrosze(1);
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(wrongCoin);
            _parkMeter.InsertCoin(Coin.FromZlotys(5));
            _parkMeter.InsertCoin(Coin.FromZlotys(1));

            Assert.Equal(ParkMeterState.AcceptingState, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_CoinSumMoreThan7zł_GiveChangeState()
        {
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(Coin.FromZlotys(5));
            _parkMeter.InsertCoin(Coin.FromZlotys(2));

            Assert.Equal(ParkMeterState.GiveChangeState, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_AfterAcceptingState_GiveChangeState()
        {
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(Coin.FromZlotys(5));
            _parkMeter.InsertCoin(Coin.FromZlotys(1));

            _parkMeter.InsertCoin(Coin.FromZlotys(1));

            Assert.Equal(ParkMeterState.GiveChangeState, _parkMeter.CheckState());
        }

        [Fact]
        public void CheckResult_CoinSumEquals7złWithWrongCoin_GiveWrongCoin()
        {
            var wrongCoin = Coin.FromGrosze(1);
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(wrongCoin);
            _parkMeter.InsertCoin(Coin.FromZlotys(5));
            _parkMeter.InsertCoin(Coin.FromZlotys(1));

            var result = _parkMeter.CheckResult();

            var expected = new List<Coin>
            {
                wrongCoin
            };
            Assert.Equal(expected, result.CoinsChange);
            Assert.Equal(ParkMeterTransaction.AcceptingStateMessage, result.Message);
        }

        [Fact]
        public void CheckResult_CoinsSumMoreThan7złWithWrongCoins_GiveWrongCoinsAndChange()
        {
            var wrongCoin = Coin.FromGrosze(1);
            var unnecessaryCoin = Coin.FromZlotys(5);
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(wrongCoin);
            _parkMeter.InsertCoin(Coin.FromZlotys(5));
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(unnecessaryCoin);

            var result = _parkMeter.CheckResult();

            var expected = new List<Coin>
            {
                unnecessaryCoin,
                wrongCoin
            };
            Assert.Equal(expected, result.CoinsChange);
            Assert.Contains("5zł", result.Message);
        }

        [Fact]
        public void CheckResult_WaitingForMoreCoins()
        {
            var result = _parkMeter.CheckResult();

            Assert.Contains("7zł", result.Message);
        }

        [Fact]
        public void GetCoinsAsString_NoCoinsInserted_ReturnsEmptyString()
        {
            Assert.Equal("", _parkMeter.GetCoinsAsString());
        }

        [Fact]
        public void GetCoinsAsString_1złInserted_Returns1zł()
        {
            _parkMeter.InsertCoin(Coin.FromZlotys(1));

            Assert.Equal("1zł", _parkMeter.GetCoinsAsString());
        }

        [Fact]
        public void GetCoinsAsString_1złAnd2złAnd5złInserted_Returns1złCOMMA2złCOMMA3zł()
        {
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(Coin.FromZlotys(2));
            _parkMeter.InsertCoin(Coin.FromZlotys(5));

            Assert.Equal("1zł, 2zł, 5zł", _parkMeter.GetCoinsAsString());
        }
    }
}
