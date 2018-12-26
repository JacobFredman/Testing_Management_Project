using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BE.Routes;
using BL;
namespace PLWPF.UserControls
{
    /// <summary>
    /// Interaction logic for AddressPicker.xaml
    /// </summary>
    public partial class AddressPicker : UserControl
    {
        public Address Address
        {
            set
            {
               
                    TexBoxAddress.Text = (value!=null)?value.ToString():"";
                    new Thread(() =>
                    {
                        try
                        {
                        var text= Routes.GetAddressSuggestionsGoogle(value.ToString()).First();
                        Action action = () =>
                        {
                            TexBoxAddress.Text = text;
                            TexBoxAddress.BorderBrush = Brushes.Black;
                        };
                        Dispatcher.BeginInvoke(action);
                        }
                        catch
                        {
                            Action act = () => {  TexBoxAddress.BorderBrush = Brushes.Red; };
                            Dispatcher.BeginInvoke(act);
                        }
                    }).Start();
            
            }
            get =>new Address(TexBoxAddress.Text);
        }
        public AddressPicker()
        {
            InitializeComponent();
        }

        public event EventHandler TextChanged;

        private void TexBoxAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TexBoxAddress.Text != "")
                {

                    var text = TexBoxAddress.Text;
                    new Thread(() =>
                    {
                        try
                        {
                            var list = Routes.GetAddressSuggestionsGoogle(text);
                            if (list.Any(x => x == text))
                            {
                                Action act = () => { ListBoxSuggestions.Visibility = Visibility.Hidden; };
                                Dispatcher.BeginInvoke(act);
                                return;
                            }

                            Action action = () =>
                            {
                                ListBoxSuggestions.ItemsSource = list;
                                ListBoxSuggestions.Visibility = Visibility.Visible;
                                ListBoxSuggestions.UnselectAll();
                                TexBoxAddress.BorderBrush = Brushes.Black;
                            };
                            Dispatcher.BeginInvoke(action);
                        }
                        catch
                        {
                            Action action = () => { TexBoxAddress.BorderBrush = System.Windows.Media.Brushes.Red; };
                            Dispatcher.BeginInvoke(action);
                        }
                    }).Start();

                    TextChanged(this, e);
                }
            }
            catch { }
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            ListBoxSuggestions.Visibility = Visibility.Collapsed;
        }

        private void ListBoxSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxSuggestions.SelectedItem != null)
            {
                TexBoxAddress.Text = (string) ListBoxSuggestions.SelectedItem;
                ListBoxSuggestions.Visibility = Visibility.Collapsed;
            }
        }
    }
}
