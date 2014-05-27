﻿using System;
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

        public override string ToString()
        {
            return string.Join("\t", _coinsGroups.Select(c => c.ToString()));
        }

        public CoinStorage Clone()
        {
            var copy = new CoinStorage();

            foreach (var c in _coinsGroups)
                copy._coinsGroups.Add(c.Clone());

            return copy;
        }

        public void Remove(List<Coin> coins)
        {
            foreach (var coin in coins)
                --_coinsGroups.Single(c => c.Coin.Equals(coin)).Count;
        }
    }
}
