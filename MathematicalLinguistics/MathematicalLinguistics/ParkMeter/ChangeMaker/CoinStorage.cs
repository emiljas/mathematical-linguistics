using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathematicalLinguistics.Utils;
using System.Collections;

namespace MathematicalLinguistics.ParkMeter.Change
{
    public class CoinStorage
    {
        private List<CoinGroup> _coinsGroups;
        public List<CoinGroup> CoinsGroups
        {
            get
            {
                return _coinsGroups;
            }
        }

        public CoinStorage()
        {
            _coinsGroups = new List<CoinGroup>();
        }

        /// <summary>
        /// Fill coin storage by specified number of coins.
        /// </summary>
        /// <param name="coin">Inserted coins type.</param>
        /// <param name="count">Number of coins to insert.</param>
        /// <returns>Coin storage to which coins were inserted.</returns>
        public CoinStorage Insert(Coin coin, int count)
        {
            var coinGroup = new CoinGroup
            {
                Coin = coin,
                Count = count
            };

            var inserted = false;
            for (int i = 0; i < _coinsGroups.Count; ++i)
            {
                if (_coinsGroups[i].Coin.Grosze < coin.Grosze)
                {
                    _coinsGroups.Insert(i, coinGroup);
                    inserted = true;
                    break;
                }
                else if (_coinsGroups[i].Coin.Equals(coin))
                {
                    _coinsGroups[i].Count += count;
                    inserted = true;
                    break;
                }
            }

            if (!inserted)
                _coinsGroups.Add(coinGroup);

            return this;
        }

        /// <summary>
        /// Convert coin storage to string.
        /// </summary>
        /// <returns>String format of storage.</returns>
        public override string ToString()
        {
            return string.Join("\t", _coinsGroups.Select(c => c.ToString()));
        }

        /// <summary>
        /// Deep copy of coin storage.
        /// </summary>
        /// <returns>Coin storage copy.</returns>
        public CoinStorage Clone()
        {
            var copy = new CoinStorage();

            foreach (var c in _coinsGroups)
                copy._coinsGroups.Add(c.Clone());

            return copy;
        }

        /// <summary>
        /// Remove coin from coin storage.
        /// </summary>
        /// <param name="coins">Coins to remove.</param>
        public void Remove(List<Coin> coins)
        {
            foreach (var coin in coins)
                --_coinsGroups.Single(c => c.Coin.Equals(coin)).Count;
        }
    }
}
