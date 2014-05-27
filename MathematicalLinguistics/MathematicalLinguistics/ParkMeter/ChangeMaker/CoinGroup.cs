using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalLinguistics.ParkMeter.Change
{
    public class CoinGroup
    {
        public Coin Coin { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return Coin.ToString() + " x " + Count;
        }

        public CoinGroup Clone()
        {
            var coinGroup = new CoinGroup();
            
            coinGroup.Coin = Coin.Clone();
            coinGroup.Count = Count;

            return coinGroup;
        }
    }
}
