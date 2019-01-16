using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using BE.Routes;
using BL;

namespace PLWPF.UserControls
{
    /// <summary>
    ///     A control to Pick an address
    /// </summary>
    public partial class AddressPicker : UserControl
    {
        /// <summary>
        /// A token for a session
        /// </summary>
        private string _token;

        /// <summary>
        /// A address picker
        /// </summary>
        public AddressPicker()
        {
            InitializeComponent();

            GenerateNewToken();
        }
       
        /// <summary>
        /// Address dependency property
        /// </summary>
        public static readonly DependencyProperty AddressProperty =
                DependencyProperty.Register("Address", typeof(Address), typeof(AddressPicker), new PropertyMetadata(null));

            /// <summary>
            ///     The Selected address
            /// </summary>
            public Address Address
        {
            set
            {
                //set the address in the text box
                TexBoxAddress.TextChanged -= TexBoxAddress_TextChanged;
                TexBoxAddress.Text = value != null ? value.ToString() : "";
                //update dp
                SetValue(AddressProperty, new Address(TexBoxAddress.Text));

                //validate address on google
                new Thread(() =>
                {
                    try
                    {
                        GenerateNewToken();
                        var text = Routes.GetAddressSuggestionsGoogle(value.ToString(), _token).Select(x=>x.Name).First();

                        //set the validated address
                        void Action()
                        {
                            SetValue(AddressProperty,new Address(text));
                            TexBoxAddress.Text = text;
                            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;
                            TexBoxAddress.BorderBrush = Brushes.LightGray;
                        }

                        Dispatcher.BeginInvoke((Action) Action);
                    }
                    catch
                    {
                        //error with address
                        void Act()
                        {
                            TexBoxAddress.BorderBrush = Brushes.Red;
                            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;
                        }

                        Dispatcher.BeginInvoke((Action) Act);
                    }
                }).Start();
            }
            get => (Address)GetValue(AddressProperty);
        }

        /// <summary>
        ///     Called on address changed
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        ///     On Text changed
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
                            var list = Routes.GetAddressSuggestionsGoogle(text, _token).Select(x=>x.Name);
                            //if the address is already typed
                            if (list.Any(x => x == text))
                            {
                                void Act()
                                {
                                    ListBoxSuggestions.Visibility = Visibility.Hidden;
                                }

                                Dispatcher.BeginInvoke((Action) Act);
                                return;
                            }

                            //open the suggestions list
                            void Action()
                            {
                                ListBoxSuggestions.ItemsSource = list;
                                ListBoxSuggestions.Visibility = Visibility.Visible;
                                ListBoxSuggestions.UnselectAll();
                                TexBoxAddress.BorderBrush = Brushes.LightGray;
                            }

                            Dispatcher.BeginInvoke((Action) Action);
                        }
                        catch
                        {
                            //make the border red if there is no internet or on invalid address
                            void Action()
                            {
                                TexBoxAddress.BorderBrush = Brushes.Red;
                            }

                            Dispatcher.BeginInvoke((Action) Action);
                        }
                    }).Start();

                    TextChanged?.Invoke(this, e);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// close suggestions on lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            ListBoxSuggestions.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// on select suggestion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (ListBoxSuggestions.SelectedItem == null) return;

            //set the selected address
            TexBoxAddress.TextChanged -= TexBoxAddress_TextChanged;
            TexBoxAddress.Text = (string) ListBoxSuggestions.SelectedItem;
            SetValue(AddressProperty, new Address(TexBoxAddress.Text));
            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;

            //remove the list
            ListBoxSuggestions.Visibility = Visibility.Collapsed;

            GenerateNewToken();

            TextChanged?.Invoke(this, e);
        }

        /// <summary>
        /// generate token for a new session
        /// </summary>
        private void GenerateNewToken()
        {
            _token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "")
                .Replace(@"\", "").Replace("/", "").Replace(".", "").Replace(":", "");
        }
    }
}