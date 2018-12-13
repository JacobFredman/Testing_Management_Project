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
        BE.MainObjects.Test _test;
        public ShowTest(BE.MainObjects.Test test)
        {
            InitializeComponent();
            _test = test;
            licenseTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.LicenseType));
            licenseTypeComboBox.SelectedItem = test.LicenseType;
            addressOfBeginningTestTextBox.Text = test.AddressOfBeginningTest.ToString();
            CriterionsDataGrid.ItemsSource = _test.Criteria;
            DataContext = _test;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ShowRoute_Click(object sender, RoutedEventArgs e)
        {
            if(_test.RouteUrl.ToString().Length>1)
                 BL.Routes.ShowUrlInChromeWindow(_test.RouteUrl);
        }
    }
}
