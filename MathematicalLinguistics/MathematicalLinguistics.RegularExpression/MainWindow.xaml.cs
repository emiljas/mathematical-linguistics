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

namespace MathematicalLinguistics.RegularExpression
{
    public partial class MainWindow : Window
    {
        private const string TwoNumbersAddingRegex = "[0-9]+[+][0-9]+";
        private const string MacAddressRegex = "([0-9A-F]{2}[:-]){5}[0-9A-F]{2}";

        private Brush _greenBrush = new SolidColorBrush(Colors.Green);
        private Brush _redBrush = new SolidColorBrush(Colors.Red);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var textToCheck = TextToCheckTextBox.Text;
            
            var textBlock = new TextBlock();
            textBlock.Text = textToCheck;

            TextsToCheckListBox.Items.Add(textBlock);

            TextToCheckTextBox.Text = "";
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            var regularExpressionChecker = new RegexExpressionChecker();
            
            string regex = "";


            if(IPAndSubnetMaskRadioButton.IsChecked.Value)
            {
            }
            else if(MACAddressRadioButton.IsChecked.Value)
            {
            }
            else if(EmailAddressRadioButton.IsChecked.Value)
            {
            }
            else if(AddingIntegersRadioButton.IsChecked.Value)
            {
            }
            else if(SubtractingComplexNumbersRadioButton.IsChecked.Value)
            {
            }
            else if(HtmlTextFormattingRadioButton.IsChecked.Value)
            {
            }
            else if (HtmlTablesRadioButton.IsChecked.Value)
            {
            }

            foreach (var item in TextsToCheckListBox.Items)
            {
                regularExpressionChecker.Compile(regex);

                var textBlock = item as TextBlock;
                bool valid = regularExpressionChecker.Check(textBlock.Text);

                if (valid)
                    textBlock.Foreground = _greenBrush;
                else
                    textBlock.Foreground = _redBrush;
            }
        }
    }
}
