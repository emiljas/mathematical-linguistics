using MathematicalLinguistics.ParkMeter.Change;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathematicalLinguistics.ParkMeter
{
    public class ParkMeterTransaction
    {
        public const string WaitingForMoreCoinsMessageFormat
            = "Waiting for {0}.";
        public const string AcceptingStateMessage
            = "Inserted coins sum equals price.";
        public const string GiveChangeMessageFormat
            = "Inserted coins sum is about {0} too high.";

        private string _coinsAsString = "";
        private Coin _currentCoin;
        private List<Coin> _validInsertedCoins = new List<Coin>();
        private ParkMeterState _state;
        Price _price = Price.FromZlotys(7);
        private int _coinsState = 0;
        public readonly static Coin[] ValidCoins = new []
        {
            Coin.FromZlotys(1),
            Coin.FromZlotys(2),
            Coin.FromZlotys(5)
        };
        private const int AcceptingStateCoinsValue = 700;

        private static readonly Dictionary<int, Dictionary<int, int>> StateTable;
        private ChangeMaker _changeMaker;
        private List<Coin> _wrongCoins = new List<Coin>();

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

        public ParkMeterTransaction(ChangeMaker changeMaker)
        {
            _changeMaker = changeMaker;
        }

        public void InsertCoin(Coin coin)
        {
            _currentCoin = coin;

            if (IsValidCoin())
            {
                _validInsertedCoins.Add(_currentCoin);
                AppendCoinToString();

                if (IsCompleted())
                {
                    AfterAcceptingState();
                    return;
                }

                _coinsState = StateTable[_coinsState][coin.Grosze];
                UpdateState();
            }
            else
            {
                _wrongCoins.Add(_currentCoin);
            }
        }

        private bool IsValidCoin()
        {
            if (ValidCoins.Contains(_currentCoin))
                return true;

            return false;
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

        public ParkMeterState CheckState()
        {
            return _state;
        }

        public ParkMeterTransactionResult CheckResult()
        {
            var result = new ParkMeterTransactionResult();
            var actual = Price.FromCoins(_validInsertedCoins);

            switch (_state)
            {
                case ParkMeterState.WaitingForMoreCoins:
                    result.CoinsChange = _wrongCoins;
                    result.Message = string.Format(WaitingForMoreCoinsMessageFormat
                                               , (_price - actual).ToString());
                    break;
                case ParkMeterState.AcceptingState:
                    result.CoinsChange = _wrongCoins;
                    result.Message = AcceptingStateMessage;
                    break;
                case ParkMeterState.GiveChangeState:
                    try
                    {
                        var change = _changeMaker.Make(_price, actual);
                        change.AddRange(_wrongCoins);

                        result.CoinsChange = change;
                        result.Message = string.Format(GiveChangeMessageFormat
                                                    , (actual - _price).ToString());
                    }
                    catch(MissingCoinsInCoinStorageException ex)
                    {
                        result.CoinsChange = _validInsertedCoins.Union(_wrongCoins).ToList();
                        result.Message = "Missing coins in coin storage. Call 333-444-555 for park meter operator";
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        public string GetCoinsAsString()
        {
            return _coinsAsString;
        }
    }
}
