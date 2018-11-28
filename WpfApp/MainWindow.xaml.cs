using BE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using BL;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void OnKlick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Button.IsEnabled = false;
                Test t = new Test(319185997, 319185989);
                string address = AddressTextBox.Text;
                t.SetRouteAndAddressToTest(new Address(address));

                if (t.RouteUrl != null)
                    Process.Start("chrome.exe", "--app=" + t.RouteUrl.AbsoluteUri);
                this.Button.IsEnabled = true;
              

            }
            catch (GoogleAddressException ex)
            {
                switch (ex.ErrorCode)
                {
                    case "CONNECTION_FAILURE":
                        MessageBox.Show("There is no internet connection. \nPlease try again later.",
                            "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "ADDRESS_FAILURE":
                        MessageBox.Show("We can't find a route for the address. \nPlease an other address.",
                            "Address error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    default:
                        MessageBox.Show("ERROR: " + ex.Message + "!!\t" + "Source:" + ex.StackTrace.Substring(5, 80) + "...",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
                
                this.Button.IsEnabled = true;

                // Console.WriteLine("ERROR: " + ex.Message + "!!\t" + "Source:" + ex.StackTrace.Substring(5, 80) + "...");
            }
        }
    }
}
