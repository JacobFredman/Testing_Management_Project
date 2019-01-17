using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        ///     Bl object
        /// </summary>
        private readonly IBL _bL = FactoryBl.GetObject;

        /// <summary>
        ///     list of showed testers
        /// </summary>
        private IEnumerable<Tester> _testerList = FactoryBl.GetObject.AllTesters;

        /// <summary>
        ///     list of showed tests
        /// </summary>
        private IEnumerable<Test> _testList = FactoryBl.GetObject.AllTests;

        /// <summary>
        ///     showed trainees
        /// </summary>
        private IEnumerable<Trainee> _traineeList = FactoryBl.GetObject.AllTrainees;

        //background worker for sending emails
        private BackgroundWorker _worker;

        /// <summary>
        ///     Administrator window c-tor
        /// </summary>
        public Administrator()
        {
            InitializeComponent();

            //Set all the data context
            RefreshData();

            //Set all the license ComBox source
            ComboBoxLicenseFilterTest.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTester.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ComboBoxLicenseFilterTrainee.ItemsSource = Enum.GetValues(typeof(LicenseType));

            //set the other comBox item source
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
            TraineeGrid.DataContext = _bL.AllTrainees;

            TesterGrid.DataContext = null;
            TesterGrid.DataContext = _bL.AllTesters;

            TestGrid.DataContext = null;
            TestGrid.DataContext = _bL.AllTests;

            //update number of objects
            NumberOfTraineesLabel.Content = _bL.AllTrainees.Count().ToString();
            NumberOfTestersLabel.Content = _bL.AllTesters.Count().ToString();
            NumberOfTestsLabel.Content = _bL.AllTests.Count().ToString();

            //update comBox source
            ComboBoxFilterSchoolTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesBySchool()
                .Where(y => y.Key != "").Select(x => x.Key);
            ComboBoxFilterTesterIdTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineesByTester()
                .Where(y => y.Key != "").Select(x => x.Key);
            ComboBoxFilterMunOfTestsTrainee.ItemsSource = FactoryBl.GetObject.GetAllTraineeByNumberOfTests()
                .Select(x => x.Key);

            //update list
            _traineeList = FactoryBl.GetObject.AllTrainees;
            _testerList = FactoryBl.GetObject.AllTesters;
            _testList = FactoryBl.GetObject.AllTests;

            //clean search textBox
            SearchTextBoxTester.Text = "";
            TextBoxSearchTest.Text = "";
            TextBoxSearchTrainee.Text = "";
        }

        /// <summary>
        ///     Open Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region Send Email Before Tests

        /// <summary>
        ///     Send Email Before Test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_OnClickEmail(object sender, RoutedEventArgs e)
        {
            try
            {
                //set the progress bar
                ProgressBarB.Visibility = Visibility.Visible;
                ProgressBarB.Maximum = _testList.Count(x => x.Passed == null && x.TestTime.Year == DateTime.Now.Year &&
                                                            x.TestTime.DayOfYear == DateTime.Now.DayOfYear);
                ProgressBarB.Minimum = 0;
                ProgressBarB.Value = 0;
                ProgressLabel.Content = "Sending Emails:  ";
                ProgressLabel.Visibility = Visibility.Visible;

                //set the background worker
                _worker = new BackgroundWorker();
                _worker.DoWork += Worker_DoWork;
                _worker.ProgressChanged += Worker_ProgressChanged;
                _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                _worker.WorkerReportsProgress = true;
                _worker.RunWorkerAsync();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     finish send email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarB.Visibility = Visibility.Collapsed;
            ProgressLabel.Visibility = Visibility.Collapsed;
            ExceptionMessage.Show((string) e.Result);
        }

        /// <summary>
        ///     progress of sending emails changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBarB.Value = e.ProgressPercentage;
        }

        /// <summary>
        ///     send emails
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //send to all tests that are today and didn't updated
                var count = _testList
                    .Where(x => x.Passed == null && x.TestTime.Year == DateTime.Now.Year &&
                                x.TestTime.DayOfYear == DateTime.Now.DayOfYear)
                    .SendEmailToAllTraineeBeforeTest(); // todo: there was ref _worker as paramter which not compiled
                
                e.Result = "You Sent " + count + " Emails";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        #endregion

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
                //open window
                var win = new AddTrainee(((Trainee) TraineeGrid.SelectedItem).Id);
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
                //validate delete
                if (!ValidationMessage.Show("Are you sure you want to delete " + trainee?.FirstName + " " +
                                            trainee?.LastName + "?"))
                    return;
                _bL.RemoveTrainee(trainee);
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
                //open window
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
            //if the the search is empty
            if (TextBoxSearchTrainee.Text == "")
            {
                TraineeGrid.DataContext = _bL.AllTrainees.ToList();
                _traineeList = _bL.AllTrainees.ToList();
            }
            else
            {
                //filter the trainees
                var text = TextBoxSearchTrainee.Text;
                new Thread(() =>
                {
                    var list = _bL.SearchTrainee(text);

                    void Act()
                    {
                        TraineeGrid.DataContext = list;
                        _traineeList = list;
                    }

                    Dispatcher.BeginInvoke((Action) Act);
                }).Start();
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

            //search in trainees
            var list = _bL.AllTrainees.Where(p =>
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
                //filter on license
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

        /// <summary>
        ///     On Clear filter click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterButtonTrainee_Click(object sender, RoutedEventArgs e)
        {
            //unselect all comBox
            ComboBoxLicenseFilterTrainee.SelectedIndex = -1;
            ComboBoxFilterSchoolTrainee.SelectedIndex = -1;
            ComboBoxFilterTesterIdTrainee.SelectedIndex = -1;
            ComboBoxFilterMunOfTestsTrainee.SelectedIndex = -1;

            RefreshData();
        }

        /// <summary>
        ///     On Select School filter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFilterSchoolTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //copy the items
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesBySchool()
                    .First(x => x.Key == (string) ComboBoxFilterSchoolTrainee.SelectedItem))
                    list.Add(item);
                //update the trainees
                TraineeGrid.DataContext = list;
                _traineeList = list;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     On select Filter Teacher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFilterTesterIdTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //copy the items
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineesByTester()
                    .First(x => x.Key == (string) ComboBoxFilterTesterIdTrainee.SelectedItem))
                    list.Add(item);
                //update the list
                TraineeGrid.DataContext = list;
                _traineeList = list;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     On number of tests changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFilterMunOfTestsTrainee_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //copy items
                var list = new List<Trainee>();
                foreach (var item in FactoryBl.GetObject.GetAllTraineeByNumberOfTests()
                    .First(x => x.Key == (int) ComboBoxFilterMunOfTestsTrainee.SelectedItem))
                    list.Add(item);
                //update list
                TraineeGrid.DataContext = list;
                _traineeList = list;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     disable update and remove in context menu when nothing is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     On press enter on grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateTraineeClick(this, new EventArgs());
                e.Handled = true;
            }
        }

        #endregion

        #region Tester

        /// <summary>
        ///     Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTesterClick(object sender, EventArgs e)
        {
            try
            {
                //open window
                var win = new AddTester(((Tester) TesterGrid.SelectedItem).Id);
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
                //validate delete
                if (!ValidationMessage.Show("Are you sure you want to delete " + tester?.FirstName + " " +
                                            tester?.LastName + "?"))
                    return;
                _bL.RemoveTester(tester);
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
                //open window
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
            //if search is empty
            if (SearchTextBoxTester.Text == "")
            {
                TesterGrid.DataContext = _bL.AllTesters.ToList();
                _testerList = _bL.AllTesters.ToList();
            }
            else
            {
                TesterGrid.DataContext = _bL.SearchTester(SearchTextBoxTester.Text);
                _testerList = _bL.SearchTester(SearchTextBoxTester.Text);
            }
        }

        /// <summary>
        ///     On button Search click ,update grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            //get the search parameters
            var id = TextBoxSearchIdTester.Text != "" ? TextBoxSearchIdTester.Text : null;
            var fName = TextBoxSearchFirstNameTester.Text != "" ? TextBoxSearchFirstNameTester.Text : null;
            var lName = TextBoxSearchLastNameTester.Text != "" ? TextBoxSearchLastNameTester.Text : null;

            //filter
            var list = _bL.AllTesters.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (fName != null && fName.ToLower() == p.FirstName.ToLower()) return true;
                if (lName != null && lName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            //update
            TesterGrid.DataContext = list;
            _testerList = list;
        }

        /// <summary>
        ///     clear all search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        //On license filter update grid
        private void ComboBoxLicenseFilterTester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //filter on license
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

        /// <summary>
        ///     On clear filter click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterButtonTester_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            ComboBoxLicenseFilterTester.SelectedIndex = -1;
        }

        /// <summary>
        ///     disable update and remove in context menu when nothing is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     On press enter on grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateTesterClick(this, new EventArgs());
                e.Handled = true;
            }
        }

        #endregion

        #region Test

        /// <summary>
        ///     Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTestClick(object sender, EventArgs e)
        {
            try
            {
                //save test details
                var test = TestGrid.SelectedItem as Test;
                var win = new EditTest(test?.Id);
                var passed = test?.Passed;

                //open window
                win.ShowDialog();
                RefreshData();

                //get trainee and updated test
                var trainee = _bL.AllTrainees.First(x => x.Id == test?.TraineeId);
                test = _bL.AllTests.First(x => x.Id == test?.Id);

                //if the passed state didn't change
                if (test.Passed == passed)
                    return;

                //Set Label
                ProgressLabel.Content = "Sending Email to " + trainee.FirstName + " " + trainee.LastName + "...";
                ProgressLabel.Visibility = Visibility.Visible;

                //Send Email to trainee
                var thread = new Thread(() =>
                {
                    try
                    {
                        Pdf.CreateLicensePdf(test, trainee);
                        Email.SentEmailToTraineeAfterTest(test, trainee);

                        void Act()
                        {
                            ExceptionMessage.Show("Successfully Send Email to " + trainee.FirstName + " " +
                                                  trainee.LastName);
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
                //validate delete
                if (!ValidationMessage.Show("Are you sure you want to delete test number " + test?.Id + " ?"))
                    return;
                _bL.RemoveTest(test);
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
            new Thread(() =>
            {
                try
                {
                    //Check internet connectivity
                    try
                    {
                        var wc = new WebClient();
                        wc.DownloadData("https://www.google.com/");
                    }
                    catch
                    {
                        throw new Exception("There is No Internet Connection.Please Try Again Later.");
                    }

                    //open window
                    void Act1()
                    {
                        var win = new EditTest();
                        win.ShowDialog();
                        RefreshData();
                    }

                    Dispatcher.BeginInvoke((Action) Act1);
                }
                catch (Exception ex)
                {
                    void Act2()
                    {
                        ExceptionMessage.Show(ex.Message, ex.ToString());
                    }

                    Dispatcher.BeginInvoke((Action) Act2);
                }
            }).Start();
        }

        /// <summary>
        ///     On search Test update grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxSearchTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if search is empty
                if (TextBoxSearchTest.Text == "")
                {
                    TestGrid.DataContext = _bL.AllTests.ToList();
                    _testList = _bL.AllTests.ToList();
                }
                else
                {
                    TestGrid.DataContext = _bL.SearchTest(TextBoxSearchTest.Text);
                    _testList = _bL.SearchTest(TextBoxSearchTest.Text);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     On Advanced search click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Get the search data
                var id = TextBoxSearchIdTest.Text != "" ? TextBoxSearchIdTest.Text : null;
                var traineeId = TextBoxSearchTraineeIDTest.Text != "" ? TextBoxSearchTraineeIDTest.Text : null;
                var testerId = TextBoxSearchTesterIDTest.Text != "" ? TextBoxSearchTesterIDTest.Text : null;

                //filter the tests
                var list = _bL.AllTests.Where(p =>
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

        /// <summary>
        ///     clear all filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        ///     On license filter click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxLicenseFilterTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //filter tests on license
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

        /// <summary>
        ///     filer test grid on selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFilterOtherTest_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selection = ComboBoxFilterOtherTest.SelectedItem.ToString();
                IEnumerable<Test> tests = new List<Test>();

                //filter according to comBox selection and put in tests
                switch (selection)
                {
                    case "Not Updated Tests":
                        tests = _bL.GetAllTestsToCome();
                        break;
                    case "Updated Tests":
                        tests = _bL.GetAllTestsThatHappened();
                        break;
                    case "Test That Passed":
                        tests = _bL.AllTests.Where(x => x.Passed != null);
                        break;
                    case "Tests That Didn't Pass":
                        tests = _bL.AllTests.Where(x => x.Passed == null);
                        break;
                }

                //update filter
                _testList = tests;
                TestGrid.DataContext = tests;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     filter on selected dates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_OnSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //filter on selected range
                var dates = CalendarFilter.SelectedDates;
                _testList = _bL.AllTests.Where(
                    x => x.TestTime >= dates.First() && x.TestTime <= dates.Last().AddHours(23));
                TestGrid.DataContext = _testList;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Clear all filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearFilterButtonTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear selections
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

        /// <summary>
        ///     disable update and remove in context menu when nothing is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     On press enter on grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateTestClick(this, new EventArgs());
                e.Handled = true;
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
                    //remove label
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

        //Export all Testers in grid to excel
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
                    //remove label
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

        //Export all Tests in grid to excel
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
                    //remove label
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
    }
}