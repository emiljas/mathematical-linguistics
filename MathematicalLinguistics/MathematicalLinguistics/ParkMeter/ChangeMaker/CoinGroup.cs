using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalLinguistics.ParkMeter.ChangeMaker
{
    public class CoinGroup
    {
        public Coin Coin { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return Coin.ToString() + " x " + Count;
        }
    }
}
