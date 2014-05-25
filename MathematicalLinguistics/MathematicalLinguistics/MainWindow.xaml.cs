using MathematicalLinguistics.ParkMeter;
using MathematicalLinguistics.ParkMeter.Change;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathematicalLinguistics
{
    public partial class MainWindow : Window
    {
        private CoinStorage _coinStorage;
        private ChangeMaker _changeMaker;
        private ParkMeterTransaction _parkMeterTransaction;
        private Coin _selectedCoin = Coin.FromZlotys(1);

        public MainWindow()
        {
            InitializeComponent();

            LoadCoinStorage();
            BindNewParkMeterTransaction();
        }

        private void LoadCoinStorage()
        {
            _coinStorage = new CoinStorage();
            _coinStorage.Insert(Coin.FromZlotys(1), 10)
                        .Insert(Coin.FromZlotys(2), 7)
                        .Insert(Coin.FromZlotys(5), 9);
            _changeMaker = new ChangeMaker(_coinStorage);
        }

        private void BindNewParkMeterTransaction()
        {
            _parkMeterTransaction = new ParkMeterTransaction(_changeMaker);
            Refresh();
        }

        private void InsertCoinButton_Click(object sender, RoutedEventArgs e)
        {
            _parkMeterTransaction.InsertCoin(_selectedCoin);
            Refresh();
        }

        private void Refresh()
        {
            CoinStorageLabel.Content = _coinStorage.ToString();

            StateTextBlock.Text = "State: " + _parkMeterTransaction.CheckState().ToString();
            InsertedCoinsTextBox.Text = _parkMeterTransaction.GetCoinsAsString();
            InsertedCoinsTextBox.ScrollToEnd();

            var result = _parkMeterTransaction.CheckResult();
            ResultMessageLabel.Content = result.Message;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            BindNewParkMeterTransaction();
        }

        private void CoinRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            _selectedCoin = Coin.Parse(radioButton.Content.ToString());
        }
    }
}
