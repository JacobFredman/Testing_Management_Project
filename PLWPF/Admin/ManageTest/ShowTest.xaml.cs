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
    /// Show test details
    /// </summary>
    public partial class ShowTest : Window
    {
        BE.MainObjects.Test _test;
        public ShowTest(BE.MainObjects.Test test)
        {
            InitializeComponent();
            //get the test
            _test = test;

            //initelize component data
            licenseTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.LicenseType));
            licenseTypeComboBox.SelectedItem = test.LicenseType;
            addressOfBeginningTestTextBox.Text = test.AddressOfBeginningTest.ToString();
            CriterionsDataGrid.ItemsSource = _test.Criteria;
            DataContext = _test;
        }

  
        /// <summary>
        /// Show test route in chrome window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowRoute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_test.RouteUrl.ToString().Length > 1)
                    BL.Routes.ShowUrlInChromeWindow(_test.RouteUrl);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
