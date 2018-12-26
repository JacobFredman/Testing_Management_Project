using System.Windows;
using System.Windows.Controls;

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
