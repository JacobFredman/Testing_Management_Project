using System;
using System.Collections.Generic;
using System.Windows;
using BE;
using BE.MainObjects;
using BL;

namespace PLWPF.Admin.ManageTest
{
    /// <summary>
    ///     Update test resoults
    /// </summary>
    public partial class UpdateTest : Window
    {
        private readonly Test _test;

        public UpdateTest(Test test)
        {
            InitializeComponent();

            //initilize components data
            _test = test;
            licenseTypeComboBox.ItemsSource = Enum.GetValues(typeof(LicenseType));
            licenseTypeComboBox.SelectedItem = _test.LicenseType;
            addressOfBeginningTestTextBox.Text = _test.AddressOfBeginningTest.ToString();
            DataContext = _test;
            if (_test.Criteria == null)
                _test.Criteria = new List<Criterion>();
            CriterionsDataGrid.ItemsSource = _test.Criteria;
            _test.ActualTestTime = DateTime.Now;
        }

        /// <summary>
        ///     show test route in chrome
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

        /// <summary>
        ///     On Add Criterion click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCriterion_Click(object sender, RoutedEventArgs e)
        {
            if (CriterionTextBox.Text != null)
            {
                //add the criterion
                _test.Criteria.Add(new Criterion(CriterionTextBox.Text, (bool) PassedCriterion.IsChecked));
                CriterionsDataGrid.Items.Refresh();
            }
        }

        /// <summary>
        ///     On update test click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //update
                FactoryBl.GetObject.UpdateTest(_test);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}