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
    /// Interaction logic for UpdateTest.xaml
    /// </summary>
    public partial class UpdateTest : Window
    {
        BE.MainObjects.Test _test;

        public UpdateTest(BE.MainObjects.Test test)
        {
            InitializeComponent();
            _test = test;
            licenseTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.LicenseType));
            licenseTypeComboBox.SelectedItem = _test.LicenseType;
            addressOfBeginningTestTextBox.Text = _test.AddressOfBeginningTest.ToString();
            DataContext = _test;
            if(_test.Criteria==null)
                _test.Criteria = new List<BE.Criterion>();
            CriterionsDataGrid.ItemsSource = _test.Criteria;

        }

        private void ShowRoute_Click(object sender, RoutedEventArgs e)
        {
            BL.Routes.ShowUrlInChromeWindow(_test.RouteUrl);
        }

        private void AddCriterion_Click(object sender, RoutedEventArgs e)
        {
            if (CriterionTextBox.Text != null)
            {
                _test.Criteria.Add(new BE.Criterion(CriterionTextBox.Text, (bool)PassedCriterion.IsChecked));
                CriterionsDataGrid.Items.Refresh();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.FactoryBl.GetObject.UpdateTest(_test);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
