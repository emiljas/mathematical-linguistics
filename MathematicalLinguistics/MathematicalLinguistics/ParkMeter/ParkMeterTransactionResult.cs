using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.ParkMeter
{
    public class ParkMeterTransactionResult
    {
        public List<Coin> CoinsChange { get; set; }
        public string Message { get; set; }
    }
}
