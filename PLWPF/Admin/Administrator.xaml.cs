using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BE;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Admin.ManageTest;
using PLWPF.Admin.ManageTester;
using PLWPF.Admin.ManageTrainee;
using PLWPF.Nofitications;

namespace PLWPF.Admin
{
    /// <summary>
    ///     The administrator Window
    /// </summary>
    public partial class Administrator : MetroWindow
    {
        //BL objects
        private readonly IBL bL = FactoryBl.GetObject;
        private IEnumerable<Tester> _testerList = FactoryBl.GetObject.AllTesters;
        private IEnumerable<Test> _testList = FactoryBl.GetObject.AllTests;
        private IEnumerable<Trainee> _traineeList = FactoryBl.GetObject.AllTrainees;


        public Administrator()
        {
            InitializeComponent();

            //Set all the data context
            RefreshData();

            //Set all the license ComBox source
            ComboBoxLicenseFilterTest.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTester.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTrainee.ItemsSource = Enum.GetValues(typeof(LicenseType));


            ComboBoxFilterOtherTest.ItemsSource = new List<string>
                {"Not Updated Tests", "Updated Tests", "Test That Passed", "Tests That Didn't Pass"};
        }

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
            _traineeList = FactoryBl.GetObject.AllTrainees;
            _testerList = FactoryBl.GetObject.AllTesters;
            _testList = FactoryBl.GetObject.AllTests;

            SearchTextBoxTester.Text = "";
            TextBoxSearchTest.Text = "";
            TextBoxSearchTrainee.Text = "";

        }

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
                // ignored
            }
        }

        //Send Email Before Test
        private void MenuItem_OnClickEmail(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressLabel.Content = "Sending Emails Before Tests ...";
                ProgressLabel.Visibility = Visibility.Visible;
                new Thread(() =>
                {
                    try
                    {
                        var count = _testList
                            .Where(x => x.Passed == null && x.TestTime.Year == DateTime.Now.Year &&
                                        x.TestTime.DayOfYear == DateTime.Now.DayOfYear)
                            .SendEmailToAllTraineeBeforeTest();

                        void Act()
                        {
                            ExceptionMessage.Show("You Sent " + count + " Emails");
                        }

                        Dispatcher.BeginInvoke((Action) Act);
                    }
                    catch (Exception ex)
                    {
                        void Act()
                        {
                            ExceptionMessage.Show(ex.Message, ex.ToString());
                        }

                        Dispatcher.BeginInvoke((Action) Act);
                    }

                    void Action()
                    {
                        ProgressLabel.Visibility = Visibility.Hidden;
                    }

                    Dispatcher.BeginInvoke((Action) Action);
                }).Start();
            }
            catch
            {
                // ignored
            }
        }

        #region Trainee

        /// <summary>
        ///     Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTraineeClick(object sender, EventArgs e)
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
                    ExceptionMessage.Show(ex.Message, ex.ToString());
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
                var trainee = TraineeGrid.SelectedItem as Trainee;
                if (!ValidationMessage.Show("Are you sure you want to delete "+trainee.FirstName+" "+trainee.LastName+"?"))
                    return;
                bL.RemoveTrainee(trainee);
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    ExceptionMessage.Show(ex.Message, ex.ToString());
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
                ExceptionMessage.Show(ex.Message, ex.ToString());
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
                _traineeList = bL.AllTrainees.ToList();
            }
            else
            {
                var text = TextBoxSearchTrainee.Text;
                (new Thread(() =>
                {
                    var list = bL.SearchTrainee(text);

                    void Act()
                    {
                        TraineeGrid.DataContext = list;
                        _traineeList = list;
                    }

                    Dispatcher.BeginInvoke((Action) Act);
                })).Start();
           
            }
        }

        /// <summary>
        ///     On Advanced Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            //get the search parameters
            var id = TextBoxSearchIdTrainee.Text != "" ? TextBoxSearchIdTrainee.Text : null;
            var fName = TextBoxSearchFirstNameTrainee.Text != "" ? TextBoxSearchFirstNameTrainee.Text : null;
            var lName = TextBoxSearchLastNameTrainee.Text != "" ? TextBoxSearchLastNameTrainee.Text : null;

            var list = bL.AllTrainees.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (fName != null && fName.ToLower() == p.FirstName.ToLower()) return true;
                if (lName != null && lName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            TraineeGrid.DataContext = list;
            _traineeList = list;
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

                _traineeList = FactoryBl.GetObject.AllTrainees.Where(x =>
                    x.LicenseTypeLearning.Any(y =>
                        y.License == (LicenseType) ComboBoxLicenseFilterTrainee.SelectedItem));
            }
            catch
            {
                // ignored
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
             
                //update list
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesBySchool()
                    .First(x => x.Key == (string) ComboBoxFilterSchoolTrainee.SelectedItem))
                    list.Add(item);
                TraineeGrid.DataContext = list;
                _traineeList = list;
            }
            catch
            {
                // ignored
            }
        }

        //On select Filter Teacher
        private void ComboBoxFilterTesterIdTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
              
                //update list
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesByTester()
                    .First(x => x.Key == (string) ComboBoxFilterTesterIdTrainee.SelectedItem))
                    list.Add(item);
                TraineeGrid.DataContext = list;
                _traineeList = list;
            }
            catch
            {
                // ignored
            }
        }

        //disable update and remove in context menu when nothing is selected
        private void TraineeGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TraineeGrid.SelectedItem == null)
            {
                MenuItemUpdateTrainee.IsEnabled = false;
                MenuItemRemoveTrainee.IsEnabled = false;
            }
            else
            {
                MenuItemUpdateTrainee.IsEnabled = true;
                MenuItemRemoveTrainee.IsEnabled = true;
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
                    ExceptionMessage.Show(ex.Message, ex.ToString());
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
                var tester = TesterGrid.SelectedItem as Tester;
                if (!ValidationMessage.Show("Are you sure you want to delete " + tester.FirstName + " " + tester.LastName + "?"))
                    return;
                bL.RemoveTester(tester);
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    ExceptionMessage.Show(ex.Message, ex.ToString());
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
                ExceptionMessage.Show(ex.Message, ex.ToString());
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
                _testerList = bL.AllTesters.ToList();
            }
            else
            {
                TesterGrid.DataContext = bL.SearchTester(SearchTextBoxTester.Text);
                _testerList = bL.SearchTester(SearchTextBoxTester.Text);
            }
        }

        //On button Search click ,update grid
        private void SearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            //get the search parameters
            var id = TextBoxSearchIdTester.Text != "" ? TextBoxSearchIdTester.Text : null;
            var fName = TextBoxSearchFirstNameTester.Text != "" ? TextBoxSearchFirstNameTester.Text : null;
            var lName = TextBoxSearchLastNameTester.Text != "" ? TextBoxSearchLastNameTester.Text : null;

            var list = bL.AllTesters.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (fName != null && fName.ToLower() == p.FirstName.ToLower()) return true;
                if (lName != null && lName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            TesterGrid.DataContext = list;
            _testerList = list;
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

                _testerList = FactoryBl.GetObject.AllTesters.Where(x =>
                    x.LicenseTypeTeaching.Any(y =>
                        y == (LicenseType) ComboBoxLicenseFilterTester.SelectedItem));
            }
            catch
            {
                // ignored
            }
        }

        //On clear filter click 
        private void ClearFilterButtonTester_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            ComboBoxLicenseFilterTester.SelectedIndex = -1;
        }

        //disable update and remove in context menu when nothing is selected
        private void TesterGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TesterGrid.SelectedItem == null)
            {
                MenuItemUpdateTester.IsEnabled = false;
                MenuItemRemoveTester.IsEnabled = false;
            }
            else
            {
                MenuItemUpdateTester.IsEnabled = true;
                MenuItemRemoveTester.IsEnabled = true;
            }
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
                
                var test = TestGrid.SelectedItem as Test;
                var win = new EditTest(test.Id);
                var passed = test.Passed;

                win.ShowDialog();
                RefreshData();

                //get trainee and updated test
                var trainee = bL.AllTrainees.First(x => x.Id == test.TraineeId);
                test = bL.AllTests.First(x => x.Id == test.Id);

                if (test.Passed == passed)
                    return;

                //Set Label
                ProgressLabel.Content = "Sending Email to " + trainee.FirstName + " " + trainee.LastName + "...";
                ProgressLabel.Visibility = Visibility.Visible;

                //Send Email
                var thread = new Thread(() =>
                {
                    try
                    {
                        Pdf.CreateLicensePdf(test, trainee);
                        Email.SentEmailToTraineeAfterTest(test, trainee);

                        void Act()
                        {
                            ExceptionMessage.Show("Successfully Send Email to " + trainee.FirstName + " " + trainee.LastName);
                        }

                        Dispatcher.BeginInvoke((Action) Act);
                    }
                    catch (Exception ex)
                    {
                        void Act()
                        {
                            ExceptionMessage.Show(ex.Message, ex.ToString());
                        }

                        Dispatcher.BeginInvoke((Action) Act);
                    }

                    void Action()
                    {
                        ProgressLabel.Visibility = Visibility.Hidden;
                    }

                    Dispatcher.BeginInvoke((Action) Action);
                    
                });
                thread.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    ExceptionMessage.Show(ex.Message, ex.ToString());
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
                var test = TestGrid.SelectedItem as Test;
                if (!ValidationMessage.Show("Are you sure you want to delete test number " + test.Id + " ?"))
                    return;
                bL.RemoveTest(test);
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    ExceptionMessage.Show(ex.Message, ex.ToString());
            }
        }

        /// <summary>
        ///     Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestClick(object sender, RoutedEventArgs e)
        {
           (new Thread(() =>
           {
               try
               {
                   try
                   {
                       //check internet connectivity
                       var wc = new WebClient();
                       wc.DownloadData("https://www.google.com/");
                   }
                   catch { throw new Exception("There is No Internet Connection.Please Try Again Later."); }

                   Action act1 = () =>
                   {
                       var win = new EditTest();
                       win.ShowDialog();
                       RefreshData();
                   };
                   Dispatcher.BeginInvoke(act1);


               }
               catch (Exception ex)
               {
                   Action act2 = () => { ExceptionMessage.Show(ex.Message, ex.ToString()); };
                   Dispatcher.BeginInvoke(act2);
               }
           })).Start();
        }

        //On search Test update grid
        private void TextBoxSearchTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TextBoxSearchTest.Text == "")
                {
                    TestGrid.DataContext = bL.AllTests.ToList();
                    _testList = bL.AllTests.ToList();
                }
                else
                {
                    TestGrid.DataContext = bL.SearchTest(TextBoxSearchTest.Text);
                    _testList = bL.SearchTest(TextBoxSearchTest.Text);
                }
            }
            catch
            {
                // ignored
            }
        }

        //On Advanced search click
        private void SearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                _testList = list;
            }
            catch
            {
                // ignored
            }
        }

        //clear all filters
        private void ClearSearchTestButton_Click(object sender, RoutedEventArgs e)
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

                _testList = FactoryBl.GetObject.AllTests.Where(x =>
                    x.LicenseType == (LicenseType) ComboBoxLicenseFilterTest.SelectedItem);
            }
            catch
            {
                // ignored
            }
        }

        //filer test grid on selection
        private void ComboBoxFilterOtherTest_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selection = ComboBoxFilterOtherTest.SelectedItem.ToString();
                IEnumerable<Test> tests = new List<Test>();
                switch (selection)
                {
                    case "Not Updated Tests":
                        tests = bL.GetAllTestsToCome();
                        break;
                    case "Updated Tests":
                        tests = bL.GetAllTestsThatHappened();
                        break;
                    case "Test That Passed":
                        tests = bL.AllTests.Where(x => x.Passed != null);
                        break;
                    case "Tests That Didn't Pass":
                        tests = bL.AllTests.Where(x => x.Passed == null);
                        break;
                }

                _testList = tests;
                TestGrid.DataContext = tests;
            }
            catch
            {
                // ignored
            }
        }

        //filter on selected dates
        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var dates = CalendarFilter.SelectedDates;
                _testList = bL.AllTests.Where(
                    x => x.TestTime >= dates.First() && x.TestTime <= dates.Last().AddHours(23));
                TestGrid.DataContext = _testList;
            }
            catch
            {
                // ignored
            }
        }

        //Clear all filters
        private void ClearFilterButtonTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxLicenseFilterTest.SelectedIndex = -1;
                ComboBoxFilterOtherTest.SelectedIndex = -1;
                CalendarFilter.SelectedDate = null;
                RefreshData();
            }
            catch
            {
                // ignored
            }
        }

        //disable update and remove in context menu when nothing is selected
        private void TestGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (TestGrid.SelectedItem == null)
                {
                    MenuItemUpdateTest.IsEnabled = false;
                    MenuItemRemoveTest.IsEnabled = false;
                }
                else
                {
                    MenuItemUpdateTest.IsEnabled = true;
                    MenuItemRemoveTest.IsEnabled = true;
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion


        #region Export to Excel

        //Export all Trainees in grid to excel
        private void ExportAllTraineesToExcel_Click(object sender, RoutedEventArgs e)
        {
            //Check if excel is installed
            var officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                ExceptionMessage.Show("Excel is Not installed. Please install Excel first.");
                return;
            }

            //update ui
            ProgressLabel.Visibility = Visibility.Visible;
            ProgressLabel.Content = "Exporting Trainees in Grid To Excel.....";
            ExportTraineeesToExcel.IsEnabled = false;

            //get trainees
            var list = _traineeList.ToList();
            new Thread(() =>
            {
                void Action()
                {
                    ProgressLabel.Visibility = Visibility.Hidden;
                    ProgressLabel.Content = "";
                    ExportTraineeesToExcel.IsEnabled = true;
                }

                try
                {
                    list.ToExcel();
                }
                catch (Exception exception)
                {
                    void Act()
                    {
                        ExceptionMessage.Show(exception.Message, exception.ToString());
                    }

                    Dispatcher.BeginInvoke((Action) Act);
                }

                Dispatcher.BeginInvoke((Action) Action);
            }).Start();
        }

        private void ExportTestersToExcel_Click(object sender, RoutedEventArgs e)
        {
            //Check if excel is installed
            var officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                ExceptionMessage.Show("Excel is Not installed. Please install Excel first.");
                return;
            }

            //update ui
            ProgressLabel.Visibility = Visibility.Visible;
            ProgressLabel.Content = "Exporting Testers in Grid To Excel.....";
            ExportTestersToExcel.IsEnabled = false;
            var list = _testerList.ToList();

            new Thread(() =>
            {
                void Action()
                {
                    ProgressLabel.Visibility = Visibility.Hidden;
                    ProgressLabel.Content = "";
                    ExportTestersToExcel.IsEnabled = true;
                }

                try
                {
                    list.ToExcel();
                }
                catch (Exception exception)
                {
                    void Act()
                    {
                        ExceptionMessage.Show(exception.Message, exception.ToString());
                    }

                    Dispatcher.BeginInvoke((Action) Act);
                }

                Dispatcher.BeginInvoke((Action) Action);
            }).Start();
        }

        private void ExportTestsToExcel_Click(object sender, RoutedEventArgs e)
        {
            //Check if excel is installed
            var officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                ExceptionMessage.Show("Excel is Not installed. Please install Excel first.");
                return;
            }

            //update ui
            ProgressLabel.Visibility = Visibility.Visible;
            ProgressLabel.Content = "Exporting Tests in Grid To Excel.....";
            ExportTestsToExcel.IsEnabled = false;
            var list = _testList.ToList();

            new Thread(() =>
            {
                void Action()
                {
                    ProgressLabel.Visibility = Visibility.Hidden;
                    ProgressLabel.Content = "";
                    ExportTestsToExcel.IsEnabled = true;
                }

                try
                {
                    list.ToExcel();
                }
                catch (Exception exception)
                {
                    void Act()
                    {
                        ExceptionMessage.Show(exception.Message, exception.ToString());
                    }

                    Dispatcher.BeginInvoke((Action) Act);
                }

                Dispatcher.BeginInvoke((Action) Action);
            }).Start();
        }

        #endregion


        private void TraineeGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateTraineeClick(this, new EventArgs());
                e.Handled = true;
            }
        }
    }
}