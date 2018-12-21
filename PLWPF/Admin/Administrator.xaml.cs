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
using BL;
using BE.MainObjects;
using BE;
using System.Threading;
using PLWPF.Admin.ManageTest;
using PLWPF.Admin.ManageTester;
using PLWPF.Admin.ManageTrainee;

namespace PLWPF.Admin
{
    /// <summary>
    /// The administrator Window
    /// </summary>
    public partial class Administrator : Window
    {
        //a BL object
        private IBL bL = FactoryBl.GetObject;
        private IEnumerable<Trainee> TraineeList = FactoryBl.GetObject.AllTrainees;
        private IEnumerable<Tester> TesterList = FactoryBl.GetObject.AllTesters;
        private IEnumerable<Test> TestList = FactoryBl.GetObject.AllTests;


        public Administrator()
        {
            InitializeComponent();

            RefreshData();
            TraineeGrid.DataContext = bL.AllTrainees;
            TesterGrid.DataContext = bL.AllTesters;
            TestGrid.DataContext = bL.AllTests;

            ComboBoxLicenseFilterTest.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTester.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTrainee.ItemsSource = Enum.GetValues(typeof(LicenseType));

            ComboBoxFilterSchoolTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesBySchool().Where(y => y.Key != "").Select(x => x.Key);
            ComboBoxFilterTesterIdTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesByTester().Where(y => y.Key != "").Select(x => x.Key);

        }
        #region Trainee

        /// <summary>
        /// Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
            var win = new AddTrainee((TraineeGrid.SelectedItem as Trainee).Id);
            win.ShowDialog();
            RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected trainee 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTrainee((TraineeGrid.SelectedItem as Trainee));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddTrainee();
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Tester

        /// <summary>
        /// Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddTester((TesterGrid.SelectedItem as Tester).Id);
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                if(ex.Message!= "Object reference not set to an instance of an object.")
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected trainee 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTester((TesterGrid.SelectedItem as Tester));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddTester();
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Test

        /// <summary>
        /// Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new EditTest((TestGrid.SelectedItem as Test).Id);

                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected trainee 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTest((TestGrid.SelectedItem as Test));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new EditTest();
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// refresh all Data Context
        /// </summary>
        private void RefreshData()
        {
            DataContext = null;
            DataContext = bL;

            TraineeGrid.DataContext = null;
            TraineeGrid.DataContext = bL.AllTrainees;

            TesterGrid.DataContext = null;
            TesterGrid.DataContext = bL.AllTesters;

            TestGrid.DataContext = null;
            TestGrid.DataContext = bL.AllTests;

            NumberOfTraineesLabel.Content = bL.AllTrainees.Count().ToString();
            NumberOfTestersLabel.Content = bL.AllTesters.Count().ToString();
            NumberOfTestsLabel.Content = bL.AllTests.Count().ToString();

            ComboBoxFilterSchoolTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesBySchool()
                .Where(y => y.Key != "").Select(x => x.Key);
            ComboBoxFilterTesterIdTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesByTester()
                .Where(y => y.Key != "").Select(x => x.Key);

            TraineeList = FactoryBl.GetObject.AllTrainees;
            TesterList = FactoryBl.GetObject.AllTesters;
            TestList = FactoryBl.GetObject.AllTests;
        }

        private void TextBoxSearchTrainee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxSearchTrainee.Text == "")
            {
                TraineeGrid.DataContext = bL.AllTrainees.ToList();
                TraineeList = bL.AllTrainees.ToList();
            }
            else
            {
                TraineeGrid.DataContext = bL.SearchTrainee(TextBoxSearchTrainee.Text);
                TraineeList = bL.SearchTrainee(TextBoxSearchTrainee.Text);
            }
        }

        private void SearchTextBoxTester_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBoxTester.Text == "")
            {
                TesterGrid.DataContext = bL.AllTesters.ToList();
                TesterList  = bL.AllTesters.ToList();
            }
            else
            {
                TesterGrid.DataContext = bL.SearchTester(SearchTextBoxTester.Text);
                TesterList = bL.SearchTester(SearchTextBoxTester.Text);

            }
        }

        private void TextBoxSearchTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxSearchTest.Text == "")
            {
                TestGrid.DataContext = bL.AllTests.ToList();
                TestList = bL.AllTests.ToList();
            }
            else
            {
                TestGrid.DataContext = bL.SearchTest(TextBoxSearchTest.Text);
                TestList = bL.SearchTest(TextBoxSearchTest.Text);
            }
        }

        private void SearchTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (TextBoxSearchIdTrainee.Text != "") ? TextBoxSearchIdTrainee.Text : null;
            string FName= (TextBoxSearchFirstNameTrainee.Text != "") ? TextBoxSearchFirstNameTrainee.Text : null;
            string LName = (TextBoxSearchLastNameTrainee.Text != "") ? TextBoxSearchLastNameTrainee.Text : null;
            var list = bL.AllTrainees.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (FName != null && FName.ToLower() == p.FirstName.ToLower()) return true;
                if (LName != null && LName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            TraineeGrid.DataContext = list;
            TraineeList = list;
        }

        private void SearchClearTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void SearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (TextBoxSearchIdTester.Text != "") ? TextBoxSearchIdTester.Text : null;
            string FName = (TextBoxSearchFirstNameTester.Text != "") ? TextBoxSearchFirstNameTester.Text : null;
            string LName = (TextBoxSearchLastNameTester.Text != "") ? TextBoxSearchLastNameTester.Text : null;
            var list = bL.AllTesters.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (FName != null && FName.ToLower() == p.FirstName.ToLower()) return true;
                if (LName != null && LName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            TesterGrid.DataContext = list;
            TesterList = list;
        }

        private void ClearSearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void SearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (TextBoxSearchIdTest.Text != "") ? TextBoxSearchIdTest.Text : null;
            string traineId = (TextBoxSearchTraineeIDTest.Text != "") ? TextBoxSearchTraineeIDTest.Text : null;
            string testerId = (TextBoxSearchTesterIDTest.Text != "") ? TextBoxSearchTesterIDTest.Text : null;
            var list = bL.AllTests.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (traineId != null && traineId.ToLower() == p.TraineeId.ToString().ToLower()) return true;
                if (testerId != null && testerId.ToLower() == p.TesterId.ToString().ToLower()) return true;
                return false;
            });
            TestGrid.DataContext = list;
            TestList = list;
        }

        private void ClaerSearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void ComboBoxLicenseFilterTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TraineeGrid.DataContext = FactoryBl.GetObject.AllTrainees.Where(x =>
                    x.LicenseTypeLearning.Any(y =>
                        y.License == (LicenseType) ComboBoxLicenseFilterTrainee.SelectedItem));
                TraineeList= FactoryBl.GetObject.AllTrainees.Where(x =>
                    x.LicenseTypeLearning.Any(y =>
                        y.License == (LicenseType)ComboBoxLicenseFilterTrainee.SelectedItem));
            }
            catch { }
        }

        private void ClearFilterButtonTrainee_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxLicenseFilterTrainee.SelectedIndex = -1;
            ComboBoxFilterSchoolTrainee.SelectedIndex = -1;
            ComboBoxFilterTesterIdTrainee.SelectedIndex = -1;

            RefreshData();
          
        }

        private void ComboBoxLicenseFilterTester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TesterGrid.DataContext = FactoryBl.GetObject.AllTesters.Where(x =>
                    x.LicenseTypeTeaching.Any(y =>
                        y == (LicenseType)ComboBoxLicenseFilterTester.SelectedItem));
                TesterList = FactoryBl.GetObject.AllTesters.Where(x =>
                    x.LicenseTypeTeaching.Any(y =>
                        y == (LicenseType)ComboBoxLicenseFilterTester.SelectedItem));
            }
            catch { }
        }

        private void ClearFilterButtonTester_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            ComboBoxLicenseFilterTester.SelectedIndex = -1;
        }

        private void ComboBoxLicenseFilterTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TestGrid.DataContext = FactoryBl.GetObject.AllTests.Where(x =>
                    x.LicenseType == (LicenseType)ComboBoxLicenseFilterTest.SelectedItem);
                TestList = FactoryBl.GetObject.AllTests.Where(x =>
                    x.LicenseType == (LicenseType)ComboBoxLicenseFilterTest.SelectedItem);
            }
            catch { }
        }

        private void ClearFilterButtonTest_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            ComboBoxLicenseFilterTest.SelectedIndex = -1;
        }

        private void ComboBoxFilterSchoolTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TraineeGrid.DataContext = FactoryBl.GetObject.GetAllTraineesBySchool()
                    .Where(x => x.Key == (string) ComboBoxFilterSchoolTrainee.SelectedItem);
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesBySchool()
                    .First(x => x.Key == (string)ComboBoxFilterSchoolTrainee.SelectedItem))
                {
                    list.Add(item);
                }

                TraineeList = list;
            }
            catch { }
        }

        private void ComboBoxFilterTesterIdTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TraineeGrid.DataContext = FactoryBl.GetObject.GetAllTraineesByTester()
                    .Where(x => x.Key == (string)ComboBoxFilterTesterIdTrainee.SelectedItem);
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesByTester()
                    .First(x => x.Key == (string)ComboBoxFilterTesterIdTrainee.SelectedItem))
                {
                    list.Add(item);
                }

                TraineeList = list;

            }
            catch { }
        }

        private void ExportAllTraineeesToExcel_Click(object sender, RoutedEventArgs e)
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Excel is Not installed. Please install Excel first.");
            }
            ProgressLabel.Content = "Exporting Trainees in Grid To Excel.....";
            ExportTraineeesToExcel.IsEnabled = false;
            var list = TraineeList.ToList();
            (new Thread(() =>
            {
                Action action = () =>
                {
                    ProgressLabel.Content = "";
                    ExportTraineeesToExcel.IsEnabled = true;
                };
                try
                {
                    list.ToExcel();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                Dispatcher.BeginInvoke(action);

            })).Start();
        }

        private void ExportTestersToExcel_Click(object sender, RoutedEventArgs e)
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Excel is Not installed. Please install Excel first.");
            }
            ProgressLabel.Content = "Exporting Testers in Grid To Excel.....";
            ExportTestersToExcel.IsEnabled = false;
            var list = TesterList.ToList();
            (new Thread(() =>
            {
                Action action = () =>
                {
                    ProgressLabel.Content = "";
                    ExportTestersToExcel.IsEnabled = true;
                };
                try
                {
                    list.ToExcel();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                Dispatcher.BeginInvoke(action);
            })).Start();
        }

        private void ExportTestsToExcel_Click(object sender, RoutedEventArgs e)
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Excel is Not installed. Please install Excel first.");
            }
            ProgressLabel.Content = "Exporting Tests in Grid To Excel.....";
            ExportTestsToExcel.IsEnabled = false;
            var list = TestList.ToList();
            (new Thread(() =>
            {
                Action action = () =>
                {
                    ProgressLabel.Content = "";
                    ExportTestsToExcel.IsEnabled = true;
                };
                try
                {
                    list.ToExcel();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                Dispatcher.BeginInvoke(action);
            })).Start();
        }
    }
}
