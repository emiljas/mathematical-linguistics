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
        private ParkMeterTransaction _parkMeterTransaction;
        private Coin _selectedCoin = new Coin(1);

        public MainWindow()
        {
            InitializeComponent();
            BindNewParkMeterTransaction();
        }

        private void BindNewParkMeterTransaction()
        {
            _parkMeterTransaction = new ParkMeterTransaction();
            Refresh();
        }

        private void InsertCoinButton_Click(object sender, RoutedEventArgs e)
        {
            _parkMeterTransaction.InsertCoin(_selectedCoin);
            Refresh();
        }

        private void Refresh()
        {
            StateTextBlock.Text = "State: " + _parkMeterTransaction.CheckState().ToString();
            InsertedCoinsTextBox.Text = _parkMeterTransaction.GetCoinsAsString();
            InsertedCoinsTextBox.ScrollToEnd();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            BindNewParkMeterTransaction();
        }

        private void _1ZłRadioButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCoin = new Coin(1);
        }

        private void _2ZłRadioButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCoin = new Coin(2);
        }

        private void _5ZłRadioButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCoin = new Coin(5);
        }
    }
}
