using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BE;
using BE.MainObjects;

namespace PLWPF.Admin.ManageTest
{
    /// <summary>
    /// Interaction logic for EditTest.xaml
    /// </summary>
    public partial class EditTest : Window
    {
        private Test _test;
        public EditTest(string id=null)
        {
            InitializeComponent();
            traineeIdComboBox.ItemsSource = BL.FactoryBl.GetObject.AllTrainees.ToList();

            if (id == null)
            {
                _test = new Test();
                actualTestTimeDatePicker.IsEnabled = false;
                TestResolutsGroupBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                _test.TestTime=DateTime.Now;
                
            }
            else
            {
                _test = BL.FactoryBl.GetObject.AllTests.First(x => x.Id == id);
                Save.Content = "Update";
                traineeIdComboBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                if (_test.Criteria ==null||_test.Criteria.Count()<10)
                     _test.Criteria = Configuration.Criterions.Select(x => new Criterion(x, false)).ToList();
                _test.ActualTestTime=DateTime.Now;
                TimePickerTest.SelectedHour = _test.TestTime.Hour;

            }
            DataContext = _test;

        }

    

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)Save.Content == "Save")
                {
                    BL.FactoryBl.GetObject.AddTest(_test);
                }
                else
                {
                    BL.FactoryBl.GetObject.UpdateTest(_test);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void TraineeIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            licenseTypeComBox.ItemsSource =
                ((Trainee) traineeIdComboBox.SelectedItem).LicenseTypeLearning.Select(x => x.License);
            licenseTypeComBox.IsEnabled = true;
            _test.TraineeId = ((Trainee) traineeIdComboBox.SelectedItem).Id;

        }

        private void LicenseTypeComBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            testerIdComboBox.ItemsSource = BL.FactoryBl.GetObject.AllTesters.Where(x =>
                x.LicenseTypeTeaching.Any(y => y == (LicenseType) licenseTypeComBox.SelectedItem));
            testerIdComboBox.IsEnabled = true;
            _test.LicenseType= (LicenseType)licenseTypeComBox.SelectedItem;
        }

        private void TesterIdComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addressOfBeginningTestTextBox.Text = ((Tester) testerIdComboBox.SelectedItem).Address.ToString();
            _test.TesterId = ((Tester)testerIdComboBox.SelectedItem).Id;
        }

        private void TimePicker_OnSelectionChanged(object sender, EventArgs e)
        {
            var date = (DateTime)testTimeDatePicker.SelectedDate;
            _test.TestTime=new DateTime(date.Year,date.Month,date.Day,TimePickerTest.SelectedHour,0,0);
        }
    }
}
