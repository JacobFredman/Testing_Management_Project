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
    /// A control to Pick an address
    /// </summary>
    public partial class AddressPicker : UserControl
    {
        private string token;

        /// <summary>
        /// The Selected address
        /// </summary>
        public Address Address
        {
            set
            {

                TexBoxAddress.TextChanged -= TexBoxAddress_TextChanged;
                TexBoxAddress.Text = (value != null) ? value.ToString() : "";

                //search address on google
                new Thread(() =>
                {
                    try
                    {
                        generateNewToken();
                        var text = Routes.GetAddressSuggestionsGoogle(value.ToString(), token).First();
                        Action action = () =>
                        {
                            TexBoxAddress.Text = text;
                            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;
                            TexBoxAddress.BorderBrush = Brushes.Black;
                        };
                        Dispatcher.BeginInvoke(action);
                    }
                    catch
                    {
                        Action act = () => {
                            TexBoxAddress.BorderBrush = Brushes.Red;
                            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;
                        };
                        Dispatcher.BeginInvoke(act);
                    }
                }).Start();

            }
            get => new Address(TexBoxAddress.Text);
        }

        //ctor
        public AddressPicker()
        {
            InitializeComponent();
            generateNewToken();
        }

        /// <summary>
        /// Called on address changed
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// On Text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TexBoxAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TexBoxAddress.Text != "")
                {

                    var text = TexBoxAddress.Text;
                    //get suggestions from google
                    new Thread(() =>
                    {
                        try
                        {
                            var list = Routes.GetAddressSuggestionsGoogle(text, token);
                            //if the address is already typed
                            if (list.Any(x => x == text))
                            {
                                Action act = () => { ListBoxSuggestions.Visibility = Visibility.Hidden; };
                                Dispatcher.BeginInvoke(act);
                                return;
                            }

                            //open the suggestions list
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
                            //make the border red if there is no internet or on invalid address
                            Action action = () => { TexBoxAddress.BorderBrush = System.Windows.Media.Brushes.Red; };
                            Dispatcher.BeginInvoke(action);
                        }
                    }).Start();

                    TextChanged(this, e);
                }
            }
            catch { }
        }

        //close suggetions on lost focus
        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            ListBoxSuggestions.Visibility = Visibility.Collapsed;
        }

        //on select suggestion
        private void ListBoxSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //set the selected address
            if (ListBoxSuggestions.SelectedItem != null)
            {
                TexBoxAddress.Text = (string)ListBoxSuggestions.SelectedItem;
                ListBoxSuggestions.Visibility = Visibility.Collapsed;
                generateNewToken();
            }
        }

        //generate token for a new session
        private void generateNewToken()
        {
            token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "")
                .Replace(@"\", "").Replace("/", "").Replace(".", "").Replace(":", "");
        }
    }
}
