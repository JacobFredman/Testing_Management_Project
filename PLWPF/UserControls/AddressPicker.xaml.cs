using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                try
                {
                    new Thread(() =>
                    {
                        var text= Routes.GetAddressSuggestionsGoogle(value.ToString()).First();
                        Action action = () =>
                        {
                            TexBoxAddress.Text = text;
                            TexBoxAddress.BorderBrush = Brushes.Black;
                        };
                        Dispatcher.BeginInvoke(action);
                    }).Start();
                }
                catch
                {
                    TexBoxAddress.BorderBrush = Brushes.Red;
                }
            }
            get =>new Address(TexBoxAddress.Text);
        }
        public AddressPicker()
        {
            InitializeComponent();
        }

        private void TexBoxAddress_TextChanged(object sender, TextChangedEventArgs e)
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
                            Action action= () =>
                            {
                                TexBoxAddress.BorderBrush = System.Windows.Media.Brushes.Red;
                            };
                            Dispatcher.BeginInvoke(action);
                        }
                    }).Start();
            

            }
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
