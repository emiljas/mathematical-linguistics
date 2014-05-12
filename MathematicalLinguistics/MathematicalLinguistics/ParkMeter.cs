using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics
{
    public class ParkMeterTransaction
    {
        private Coin _currentCoin;
        private ParkMeterState _state;
        private int _coinsState = 0;
        private int[] _validCoins = new [] { 1, 2, 5 };
        private const int AcceptingStateCoinsValue = 7;

        private static readonly Dictionary<int, Dictionary<int, int>> StateTable;

        static ParkMeterTransaction()
        {
            StateTable = new Dictionary<int, Dictionary<int, int>>();

            AppendToStateTable(0, 1, 1);
            AppendToStateTable(0, 2, 2);
            AppendToStateTable(0, 5, 5);
            AppendToStateTable(1, 1, 2);
            AppendToStateTable(1, 2, 3);
            AppendToStateTable(1, 5, 6);
            AppendToStateTable(2, 1, 3);
            AppendToStateTable(2, 2, 4);
            AppendToStateTable(2, 5, 7);
            AppendToStateTable(3, 1, 4);
            AppendToStateTable(3, 2, 5);
            AppendToStateTable(3, 5, 8);
            AppendToStateTable(4, 1, 5);
            AppendToStateTable(4, 2, 6);
            AppendToStateTable(4, 5, 9);
            AppendToStateTable(5, 1, 6);
            AppendToStateTable(5, 2, 7);
            AppendToStateTable(5, 5, 10);
            AppendToStateTable(6, 1, 7);
            AppendToStateTable(6, 2, 8);
            AppendToStateTable(6, 5, 11);
        }

        private static void AppendToStateTable(int startState, int insertedCoin, int nextState)
        {
            Dictionary<int, int> dictionary;
            StateTable.TryGetValue(startState, out dictionary);

            if (dictionary == default(Dictionary<int, int>))
            {
                dictionary = new Dictionary<int, int>();
                StateTable.Add(startState, dictionary);
            }

            dictionary.Add(insertedCoin, nextState);
        }

        public void InsertCoin(Coin coin)
        {
            _currentCoin = coin;
            ValidateCoin();

            if (IsCompleted())
            {
                AfterAcceptingState();
                return;
            }

            _coinsState = StateTable[_coinsState][coin.Value];
            UpdateState();
        }

        private void AfterAcceptingState()
        {
            _state = ParkMeterState.RejectState;
        }

        private void UpdateState()
        {
            if (_coinsState > AcceptingStateCoinsValue)
                _state = ParkMeterState.RejectState;
            else if (_coinsState == AcceptingStateCoinsValue)
                _state = ParkMeterState.AcceptingState;
            else
                _state = ParkMeterState.WaitingForMoreCoins;
        }

        private bool IsCompleted()
        {
            return _state == ParkMeterState.RejectState
                || _state == ParkMeterState.AcceptingState;
        }

        private void ValidateCoin()
        {
            if (!_validCoins.Contains(_currentCoin.Value))
                throw new NotSupportedCoinException();
        }

        public ParkMeterState CheckState()
        {
            return _state;
        }
    }
}
