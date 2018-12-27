using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Admin.ManageTest;
using PLWPF.Admin.ManageTester;
using PLWPF.Admin.ManageTrainee;

namespace PLWPF.Admin
{
    /// <summary>
    ///     The administrator Window
    /// </summary>
    public partial class Administrator : MetroWindow
    {
        //BL objects
        private readonly IBL bL = FactoryBl.GetObject;
        private IEnumerable<Trainee> TraineeList = FactoryBl.GetObject.AllTrainees;
        private IEnumerable<Tester> TesterList = FactoryBl.GetObject.AllTesters;
        private IEnumerable<Test> TestList = FactoryBl.GetObject.AllTests;


        public Administrator()
        {
            InitializeComponent();

            //Set all the data context
            RefreshData();

            //Set all the license ComBox source
            ComboBoxLicenseFilterTest.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTester.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTrainee.ItemsSource = Enum.GetValues(typeof(LicenseType));
        }

        #region Trainee

        /// <summary>
        ///     Update selected Trainee in a new window
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
        ///     Remove selected trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTrainee(TraineeGrid.SelectedItem as Trainee);
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///     Add new trainee
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

        /// <summary>
        ///     Update the Grid on search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     On Advanced Sreach button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            //get the search parameters
            var id = TextBoxSearchIdTrainee.Text != "" ? TextBoxSearchIdTrainee.Text : null;
            var FName = TextBoxSearchFirstNameTrainee.Text != "" ? TextBoxSearchFirstNameTrainee.Text : null;
            var LName = TextBoxSearchLastNameTrainee.Text != "" ? TextBoxSearchLastNameTrainee.Text : null;

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

        /// <summary>
        ///     Clear search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchClearTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        ///     On filter License
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxLicenseFilterTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TraineeGrid.DataContext = FactoryBl.GetObject.AllTrainees.Where(x =>
                    x.LicenseTypeLearning.Any(y =>
                        y.License == (LicenseType) ComboBoxLicenseFilterTrainee.SelectedItem));

                TraineeList = FactoryBl.GetObject.AllTrainees.Where(x =>
                    x.LicenseTypeLearning.Any(y =>
                        y.License == (LicenseType) ComboBoxLicenseFilterTrainee.SelectedItem));
            }
            catch
            {
            }
        }

        //On Clear filter click
        private void ClearFilterButtonTrainee_Click(object sender, RoutedEventArgs e)
        {
            //unselect all comBox
            ComboBoxLicenseFilterTrainee.SelectedIndex = -1;
            ComboBoxFilterSchoolTrainee.SelectedIndex = -1;
            ComboBoxFilterTesterIdTrainee.SelectedIndex = -1;

            RefreshData();
        }

        //On Select School filter
        private void ComboBoxFilterSchoolTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TraineeGrid.DataContext = FactoryBl.GetObject.GetAllTraineesBySchool()
                    .Where(x => x.Key == (string) ComboBoxFilterSchoolTrainee.SelectedItem);

                //update list
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesBySchool()
                    .First(x => x.Key == (string) ComboBoxFilterSchoolTrainee.SelectedItem))
                    list.Add(item);

                TraineeList = list;
            }
            catch
            {
            }
        }

        //On select Filter Teacher
        private void ComboBoxFilterTesterIdTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TraineeGrid.DataContext = FactoryBl.GetObject.GetAllTraineesByTester()
                    .Where(x => x.Key == (string) ComboBoxFilterTesterIdTrainee.SelectedItem);

                //update list
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesByTester()
                    .First(x => x.Key == (string) ComboBoxFilterTesterIdTrainee.SelectedItem))
                    list.Add(item);

                TraineeList = list;
            }
            catch
            {
            }
        }

        #endregion

        #region Tester

        /// <summary>
        ///     Update selected Trainee in a new window
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
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///     Remove selected trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTester(TesterGrid.SelectedItem as Tester);
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///     Add new trainee
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

        /// <summary>
        ///     On search in tester update grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBoxTester_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBoxTester.Text == "")
            {
                TesterGrid.DataContext = bL.AllTesters.ToList();
                TesterList = bL.AllTesters.ToList();
            }
            else
            {
                TesterGrid.DataContext = bL.SearchTester(SearchTextBoxTester.Text);
                TesterList = bL.SearchTester(SearchTextBoxTester.Text);
            }
        }

        //On button Search click ,update grid
        private void SearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            //get the search parameters
            var id = TextBoxSearchIdTester.Text != "" ? TextBoxSearchIdTester.Text : null;
            var FName = TextBoxSearchFirstNameTester.Text != "" ? TextBoxSearchFirstNameTester.Text : null;
            var LName = TextBoxSearchLastNameTester.Text != "" ? TextBoxSearchLastNameTester.Text : null;

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

        //clear all search
        private void ClearSearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        //On license filter update grid
        private void ComboBoxLicenseFilterTester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TesterGrid.DataContext = FactoryBl.GetObject.AllTesters.Where(x =>
                    x.LicenseTypeTeaching.Any(y =>
                        y == (LicenseType) ComboBoxLicenseFilterTester.SelectedItem));

                TesterList = FactoryBl.GetObject.AllTesters.Where(x =>
                    x.LicenseTypeTeaching.Any(y =>
                        y == (LicenseType) ComboBoxLicenseFilterTester.SelectedItem));
            }
            catch
            {
            }
        }

        //On clear filter click 
        private void ClearFilterButtonTester_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            ComboBoxLicenseFilterTester.SelectedIndex = -1;
        }

        #endregion

        #region Test

        /// <summary>
        ///     Update selected Trainee in a new window
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
        ///     Remove selected trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTest(TestGrid.SelectedItem as Test);
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///     Add new trainee
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

        //On search Test update grid
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

        //On Advanced search click
        private void SearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            //Get the search data
            var id = TextBoxSearchIdTest.Text != "" ? TextBoxSearchIdTest.Text : null;
            var traineeId = TextBoxSearchTraineeIDTest.Text != "" ? TextBoxSearchTraineeIDTest.Text : null;
            var testerId = TextBoxSearchTesterIDTest.Text != "" ? TextBoxSearchTesterIDTest.Text : null;

            var list = bL.AllTests.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (traineeId != null && traineeId.ToLower() == p.TraineeId.ToString().ToLower()) return true;
                if (testerId != null && testerId.ToLower() == p.TesterId.ToString().ToLower()) return true;
                return false;
            });

            TestGrid.DataContext = list;
            TestList = list;
        }

        //clear all filters
        private void ClaerSearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        //On license filter click
        private void ComboBoxLicenseFilterTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TestGrid.DataContext = FactoryBl.GetObject.AllTests.Where(x =>
                    x.LicenseType == (LicenseType) ComboBoxLicenseFilterTest.SelectedItem);

                TestList = FactoryBl.GetObject.AllTests.Where(x =>
                    x.LicenseType == (LicenseType) ComboBoxLicenseFilterTest.SelectedItem);
            }
            catch
            {
            }
        }

        //Clear all filters
        private void ClearFilterButtonTest_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            ComboBoxLicenseFilterTest.SelectedIndex = -1;
        }

        #endregion

        /// <summary>
        ///     Refresh all Data Context
        /// </summary>
        private void RefreshData()
        {
            //update data grid source
            TraineeGrid.DataContext = null;
            TraineeGrid.DataContext = bL.AllTrainees;

            TesterGrid.DataContext = null;
            TesterGrid.DataContext = bL.AllTesters;

            TestGrid.DataContext = null;
            TestGrid.DataContext = bL.AllTests;

            //update number of objects
            NumberOfTraineesLabel.Content = bL.AllTrainees.Count().ToString();
            NumberOfTestersLabel.Content = bL.AllTesters.Count().ToString();
            NumberOfTestsLabel.Content = bL.AllTests.Count().ToString();

            //update comBox source
            ComboBoxFilterSchoolTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesBySchool()
                .Where(y => y.Key != "").Select(x => x.Key);
            ComboBoxFilterTesterIdTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesByTester()
                .Where(y => y.Key != "").Select(x => x.Key);

            //update list
            TraineeList = FactoryBl.GetObject.AllTrainees;
            TesterList = FactoryBl.GetObject.AllTesters;
            TestList = FactoryBl.GetObject.AllTests;
        }


        #region Export to Excel

        //Export all Trainees in grid to excel
        private void ExportAllTraineeesToExcel_Click(object sender, RoutedEventArgs e)
        {
            //Check if excel is installed
            var officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null) MessageBox.Show("Excel is Not installed. Please install Excel first.");

            //update ui
            ProgressLabel.Content = "Exporting Trainees in Grid To Excel.....";
            ExportTraineeesToExcel.IsEnabled = false;

            //get trainees
            var list = TraineeList.ToList();
            new Thread(() =>
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
            }).Start();
        }

        private void ExportTestersToExcel_Click(object sender, RoutedEventArgs e)
        {
            //Check if excel is installed
            var officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null) MessageBox.Show("Excel is Not installed. Please install Excel first.");

            //update ui
            ProgressLabel.Content = "Exporting Testers in Grid To Excel.....";
            ExportTestersToExcel.IsEnabled = false;
            var list = TesterList.ToList();

            new Thread(() =>
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
            }).Start();
        }

        private void ExportTestsToExcel_Click(object sender, RoutedEventArgs e)
        {
            //Check if excel is installed
            var officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null) MessageBox.Show("Excel is Not installed. Please install Excel first.");

            //update ui
            ProgressLabel.Content = "Exporting Tests in Grid To Excel.....";
            ExportTestsToExcel.IsEnabled = false;
            var list = TestList.ToList();

            new Thread(() =>
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
            }).Start();
        }

        #endregion

        //Open Settings
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new Settings();
                win.ShowDialog();
            }
            catch
            {
            }
        }
    }
}