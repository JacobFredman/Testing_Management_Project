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
using System.Windows.Shapes;

namespace PLWPF.Admin.ManageTest
{
    /// <summary>
    /// Interaction logic for ShowTest.xaml
    /// </summary>
    public partial class ShowTest : Window
    {
        public ShowTest(BE.MainObjects.Test test)
        {
            InitializeComponent();
            licenseTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.LicenseType));
            licenseTypeComboBox.SelectedItem = test.LicenseType;
            addressOfBeginningTestTextBox.Text = test.AddressOfBeginningTest.ToString();
            DataContext = test;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

     
    }
}
