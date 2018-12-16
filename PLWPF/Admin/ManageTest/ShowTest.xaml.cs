using System;
using System.Windows;
using BE;
using BE.MainObjects;
using BL;

namespace PLWPF.Admin.ManageTest
{
    /// <summary>
    ///     Show test details
    /// </summary>
    public partial class ShowTest : Window
    {
        private readonly Test _test;

        public ShowTest(Test test)
        {
            InitializeComponent();
            //get the test
            _test = test;

            //initelize component data
            licenseTypeComboBox.ItemsSource = Enum.GetValues(typeof(LicenseType));
            licenseTypeComboBox.SelectedItem = test.LicenseType;
            addressOfBeginningTestTextBox.Text = test.AddressOfBeginningTest.ToString();
            CriterionsDataGrid.ItemsSource = _test.Criteria;
            DataContext = _test;
        }


        /// <summary>
        ///     Show test route in chrome window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowRoute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_test.RouteUrl.ToString().Length > 1)
                    Routes.ShowUrlInChromeWindow(_test.RouteUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Route not set.");
            }
        }
    }
}