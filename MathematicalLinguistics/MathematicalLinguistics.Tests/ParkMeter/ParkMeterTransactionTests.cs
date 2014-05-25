using MathematicalLinguistics.ParkMeter;
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
        private ParkMeterTransaction _parkMeter = new ParkMeterTransaction();

        [Fact(Skip="wrong coins should be ignore")]
        public void InsertCoin_NotSupportedCoin_ThrowsNotSupportedCoinException()
        {
            var coin = Coin.FromGrosze(2);
            Assert.ThrowsDelegate insertCoin = () => _parkMeter.InsertCoin(coin);
            Assert.Throws<NotSupportedCoinException>(insertCoin);
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

        [Fact(Skip="change form of validcoins")]
        public void InsertCoin_CoinSumEquals7złWithWrongCoin_AcceptingState()
        {
            _parkMeter.InsertCoin(Coin.FromZlotys(1));
            _parkMeter.InsertCoin(Coin.FromGrosze(1)); //wrong coin
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
