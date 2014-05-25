using MathematicalLinguistics.ParkMeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.ParkMeter.ChangeMaker
{
    public class ChangeMaker
    {
        private CoinStorage _storage;

        public ChangeMaker(CoinStorage storage)
        {
            _storage = storage;
        }

        public List<Coin> Make(Price price, Price actual)
        {
            var change = (actual - price).Grosze;
            var coins = new List<Coin>();

            var i = 0;
            var currentCoinsGroup = _storage.CoinsGroups[i];

            while (change > 0)
            {
                if (currentCoinsGroup.Count == 0)
                {
                    currentCoinsGroup = _storage.CoinsGroups[++i];
                    continue;
                }

                if (change - currentCoinsGroup.Coin.Grosze >= 0)
                {
                    coins.Add(Coin.FromGrosze(currentCoinsGroup.Coin.Grosze));
                    change -= currentCoinsGroup.Coin.Grosze;

                    --currentCoinsGroup.Count;
                }
                else
                    currentCoinsGroup = _storage.CoinsGroups[++i];
            }

            return coins;
        }
    }
}
