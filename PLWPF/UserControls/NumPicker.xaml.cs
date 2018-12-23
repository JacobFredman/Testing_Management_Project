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

namespace PLWPF.UserControls
{
    /// <summary>
    /// Interaction logic for NumPicker.xaml
    /// </summary>
    public partial class NumPicker : UserControl
    {
        private uint number = 0;
        public uint Number
        {
            set
            {
                TextBoxInput.Text = value.ToString();
                number = value;
            }
            get => number;
        }

        public NumPicker()
        {
            InitializeComponent();
            TextBoxInput.Text = number.ToString();
        }

        private void TextBoxInput_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
               number= uint.Parse(TextBoxInput.Text);
            }
            catch
            {
                TextBoxInput.Text = number.ToString();
            }
        }

        private void TextBoxInput_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxInput.SelectAll();
        }
    }
}
