using MathematicalLinguistics.ParkMeter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.ParkMeter.Change
{
    public class ChangeMaker
    {
        private CoinStorage _storage;
        public CoinStorage CoinStorage
        {
            get
            {
                return _storage;
            }
        }
        private CoinStorage _notCommitedStorage;

        private int _change;
        private List<Coin> _coins;
        private CoinGroup _coinGroup;
        private int _coinGroupIndex;

        public ChangeMaker(CoinStorage storage)
        {
            _storage = storage;
        }

        public List<Coin> Make(Price price, Price actual)
        {
            _notCommitedStorage = _storage.Clone();

            _change = (actual - price).Grosze;
            _coins = new List<Coin>();

            _coinGroupIndex = 0;

            if (_notCommitedStorage.CoinsGroups.Count == 0)
                throw new MissingCoinsInCoinStorageException();
            
            _coinGroup = _notCommitedStorage.CoinsGroups[_coinGroupIndex];

            while (_change > 0)
                TryGiveMostValuableCoin();

            return _coins;
        }

        private void TryGiveMostValuableCoin()
        {
            if (_coinGroupIndex == _notCommitedStorage.CoinsGroups.Count - 1 && _coinGroup.Count == 0)
                throw new MissingCoinsInCoinStorageException();
            
            if (_coinGroup.Count == 0 || _change - _coinGroup.Coin.Grosze < 0)
                NextCoinGroup();
            else
            {
                _coins.Add(Coin.FromGrosze(_coinGroup.Coin.Grosze));
                _change -= _coinGroup.Coin.Grosze;

                --_coinGroup.Count;
            }
        }

        private void NextCoinGroup()
        {
            _coinGroup = _notCommitedStorage.CoinsGroups[++_coinGroupIndex];
        }

        public void Commit()
        {
            _storage = _notCommitedStorage;
        }

        public void RemoveFromCoinStorage(List<Coin> coins)
        {
            _notCommitedStorage = _storage;
            _notCommitedStorage.Remove(coins);
        }
    }
}
