using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.Tests.ParkMeter
{
    public class ChangeMaker
    {
        private int[] _availableCoins;

        public ChangeMaker(int[] availableCoins)
        {
            _availableCoins = availableCoins.OrderByDescending(o => o)
                                            .ToArray();
        }

        public int[] Make(int price, int actual)
        {
            return new int[] { price - actual };
        }
    }
}
