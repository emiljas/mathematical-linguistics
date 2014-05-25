using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.ParkMeter
{
    public class ParkMeterTransaction
    {
        private string _coinsAsString = "";
        private Coin _currentCoin;
        private ParkMeterState _state;
        private int _coinsState = 0;
        public readonly static Coin[] ValidCoins = new []
        {
            Coin.FromZlotys(1),
            Coin.FromZlotys(2),
            Coin.FromZlotys(5)
        };
        private const int AcceptingStateCoinsValue = 700;

        private static readonly Dictionary<int, Dictionary<int, int>> StateTable;

        static ParkMeterTransaction()
        {
            StateTable = new Dictionary<int, Dictionary<int, int>>();

            AppendToStateTable(0, 100, 100);
            AppendToStateTable(0, 200, 200);
            AppendToStateTable(0, 500, 500);
            AppendToStateTable(100, 100, 200);
            AppendToStateTable(100, 200, 300);
            AppendToStateTable(100, 500, 600);
            AppendToStateTable(200, 100, 300);
            AppendToStateTable(200, 200, 400);
            AppendToStateTable(200, 500, 700);
            AppendToStateTable(300, 100, 400);
            AppendToStateTable(300, 200, 500);
            AppendToStateTable(300, 500, 800);
            AppendToStateTable(400, 100, 500);
            AppendToStateTable(400, 200, 600);
            AppendToStateTable(400, 500, 900);
            AppendToStateTable(500, 100, 600);
            AppendToStateTable(500, 200, 700);
            AppendToStateTable(500, 500, 1000);
            AppendToStateTable(600, 100, 700);
            AppendToStateTable(600, 200, 800);
            AppendToStateTable(600, 500, 1100);
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

            AppendCoinToString();
            ValidateCoin();

            if (IsCompleted())
            {
                AfterAcceptingState();
                return;
            }

            _coinsState = StateTable[_coinsState][coin.Grosze];
            UpdateState();
        }

        private void AppendCoinToString()
        {
            if (_coinsAsString == "")
                _coinsAsString = _currentCoin.ToString();
            else
                _coinsAsString = string.Join(", ", _coinsAsString, _currentCoin.ToString());
        }

        private void AfterAcceptingState()
        {
            _state = ParkMeterState.GiveChangeState;
        }

        private void UpdateState()
        {
            if (_coinsState > AcceptingStateCoinsValue)
                _state = ParkMeterState.GiveChangeState;
            else if (_coinsState == AcceptingStateCoinsValue)
                _state = ParkMeterState.AcceptingState;
            else
                _state = ParkMeterState.WaitingForMoreCoins;
        }

        private bool IsCompleted()
        {
            return _state == ParkMeterState.GiveChangeState
                || _state == ParkMeterState.AcceptingState;
        }

        private void ValidateCoin()
        {
            if (!ValidCoins.Contains(_currentCoin))
                throw new NotSupportedCoinException();
        }

        public ParkMeterState CheckState()
        {
            return _state;
        }

        public ParkMeterTransactionResult CheckResult()
        {
            throw new NotImplementedException();
        }

        public string GetCoinsAsString()
        {
            return _coinsAsString;
        }
    }
}
