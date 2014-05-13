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

        [Fact]
        public void InsertCoin_NotSupportedCoin_ThrowsNotSupportedCoinException()
        {
            var coin = new Coin(3);
            Assert.ThrowsDelegate insertCoin = () => _parkMeter.InsertCoin(coin);
            Assert.Throws<NotSupportedCoinException>(insertCoin);
        }

        [Fact]
        public void WaitingForMoreCoinsIfNoCoinInserted()
        {
            Assert.Equal(ParkMeterState.WaitingForMoreCoins, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_CoinSumEquals7_AcceptingState()
        {
            _parkMeter.InsertCoin(new Coin(1));
            _parkMeter.InsertCoin(new Coin(5));
            _parkMeter.InsertCoin(new Coin(1));

            Assert.Equal(ParkMeterState.AcceptingState, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_CoinSumMoreThan7_RejectState()
        {
            _parkMeter.InsertCoin(new Coin(1));
            _parkMeter.InsertCoin(new Coin(5));
            _parkMeter.InsertCoin(new Coin(2));

            Assert.Equal(ParkMeterState.RejectState, _parkMeter.CheckState());
        }

        [Fact]
        public void InsertCoin_AfterAcceptingState_RejectState()
        {
            _parkMeter.InsertCoin(new Coin(1));
            _parkMeter.InsertCoin(new Coin(5));
            _parkMeter.InsertCoin(new Coin(1));

            _parkMeter.InsertCoin(new Coin(1));

            Assert.Equal(ParkMeterState.RejectState, _parkMeter.CheckState());
        }
    }
}
