using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MathematicalLinguistics
{
    public enum CoinRadioButtonState
    {
        Correct,
        Incorrect
    }

    public class CoinRadioButton : RadioButton
    {
        public int Value { get; set; }

        public CoinRadioButton(Grid grid, int row, int column, int value, CoinRadioButtonState state)
        {

            this.Style = Resources["IncorrectCoin"] as Style;
            grid.Children.Add(this);
            Grid.SetRow(grid, row);
            this.Style = Resources["IncorrectCoin"] as Style;
            Grid.SetColumn(grid, column);
            Value = value;
            this.Style = Resources["IncorrectCoin"] as Style;
            Content = ToString();
            this.Style = Resources["IncorrectCoin"] as Style;
            
        }

        public string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
