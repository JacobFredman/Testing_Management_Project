using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.MainObjects;
using BE.Routes;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Nofitications;

namespace PLWPF.Admin.ManageTest
{
    /// <summary>
    ///     Interaction logic for EditTest.xaml
    /// </summary>
    public partial class EditTest : MetroWindow
    {
        /// <summary>
        ///     error messages
        /// </summary>
        private readonly List<string> _errorMessage = new List<string>();

        /// <summary>
        ///     notifications
        /// </summary>
        private readonly List<string> _notifications = new List<string>();

        /// <summary>
        ///     the test
        /// </summary>
        private readonly Test _test;

        /// <summary>
        ///     c-tor for edit test
        /// </summary>
        /// <param name="id"></param>
        public EditTest(string id = null)
        {
            InitializeComponent();


            //If the window adds a new test
            if (id == null)
            {
                _test = new Test();

                //disable the relevant controls
                actualTestTimeDatePicker.IsEnabled = false;
                actualTimePickerTest.IsEnabled = false;
                TestResolutsGroupBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                TimePickerTest.IsEnabled = false;

                //set the source of the trainees
                traineeIdComboBox.ItemsSource = FactoryBl.GetObject.AllTrainees
                    .Where(x => x.LicenseTypeLearning.Any(y => y.ReadyForTest)).ToList();

                AddMessage("Please Select Trainee");
                //add even for uc
                addressOfBeginningTestTextBox.TextChanged += AddressOfBeginningTestTextBox_TextChanged;

                //set default date
                _test.TestTime = DateTime.Now;
                Save.IsEnabled = false;

                //set title
                Title = "Set New Test";
            }
            //if the window is to update test
            else
            {
                //get the sets
                _test = FactoryBl.GetObject.AllTests.First(x => x.Id == id);

                //remove unnecessary events
                testerIdComboBox.SelectionChanged -= TesterIdComboBox_OnSelectionChanged;
                traineeIdComboBox.SelectionChanged -= TraineeIdComboBox_SelectionChanged;
                licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                testTimeDatePicker.CalendarOpened -= TestTimeDatePicker_OnCalendarOpened;
                TimePickerTest.SelectionChanged -= TimePicker_OnSelectionChanged;

                //set the test data in the details
                traineeIdComboBox.ItemsSource = new List<Trainee>
                    {FactoryBl.GetObject.AllTrainees.First(x => x.Id == _test.TraineeId)};
                traineeIdComboBox.SelectedIndex = 0;
                testerIdComboBox.ItemsSource = new List<Tester>
                    {FactoryBl.GetObject.AllTesters.First(x => x.Id == _test.TesterId)};
                testerIdComboBox.SelectedIndex = 0;
                licenseTypeComBox.ItemsSource = new List<LicenseType?> {_test.LicenseType};
                licenseTypeComBox.SelectedIndex = 0;
                TimePickerTest.SelectedHour = _test.TestTime.Hour;
                addressOfBeginningTestTextBox.Address = _test.AddressOfBeginningTest;
                //default update data
                _test.ActualTestTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, 0, 0);
                actualTimePickerTest.SelectedHour = _test.ActualTestTime.Hour;
                //Blackout past days
                actualTestTimeDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                    DateTime.Now.AddDays(-5)));
                //Get the criteria
                if (_test.Criteria == null || _test.Criteria.Count < 10)
                    _test.Criteria = Configuration.Criteria.Select(x => new Criterion(x)).ToList();

                //disable the relevant controls
                traineeIdComboBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                TimePickerTest.IsEnabled = false;
                addressOfBeginningTestTextBox.IsEnabled = false;
                Save.IsEnabled = true;

                //add event for uc
                actualTimePickerTest.SelectionChanged += ActualTimePickerTestOnSelectionChanged;

                //update titles
                Save.Content = "Update";
                Title = "Update Test";
            }

            //set data context
            DataContext = _test;

            //set the url button
            if (_test.RouteUrl == null) ShowRouteUrlButton.IsEnabled = false;
        }

        /// <summary>
        ///     Save the Test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check address
                if (addressOfBeginningTestTextBox.Address.ToString() == "")
                    throw new Exception("Please Select an Address for the test");

                //Save or update the test
                if ((string) Save.Content == "Save")
                    FactoryBl.GetObject.AddTest(_test);
                else
                    FactoryBl.GetObject.UpdateTest(_test);
                Close();
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message, ex.ToString());
            }
        }

        #region Details

        /// <summary>
        ///     When user Selects Trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Clean License and Tester ComBox
                licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                testerIdComboBox.SelectedValue = -1;
                licenseTypeComBox.SelectedValue = -1;
                licenseTypeComBox.ItemsSource = ((Trainee) traineeIdComboBox.SelectedItem).LicenseTypeLearning
                    .Where(y => y.ReadyForTest)
                    .Select(x => x.License).ToList();
                licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;
                testerIdComboBox.ItemsSource = null;

                //Get All the Trainees that are ready for test
                _test.TraineeId = ((Trainee) traineeIdComboBox.SelectedItem).Id;
                addressOfBeginningTestTextBox.Address = ((Trainee) traineeIdComboBox.SelectedItem).Address;

                //enable the license and disable the other
                licenseTypeComBox.IsEnabled = true;
                testerIdComboBox.IsEditable = false;
                TimePickerTest.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                Save.IsEnabled = false;

                //set new Message
                ClearAllMessages();
                AddMessage("Please Select license.");

                //Focus on license
                licenseTypeComBox.Focus();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     When user Selects License type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseTypeComBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Show wait message
                ClearAllMessages();
                AddMessage("Searching for Tester. Please wait.");
                licenseTypeComBox.IsEnabled = false;
                traineeIdComboBox.IsEnabled = false;
                addressOfBeginningTestTextBox.IsEnabled = false;
                ProgressRing.IsActive = true;

                try
                {
                    testerIdComboBox.SelectedIndex = -1;
                }
                catch
                {
                    // ignored
                }

                //Get the data
                var address = addressOfBeginningTestTextBox.Address;
                var license = (LicenseType) licenseTypeComBox.SelectedItem;
                _test.LicenseType = license;

                //find all available testers
                new Thread(() =>
                {
                    IEnumerable<Tester> testers = new List<Tester>();
                    try
                    {
                        testers = FactoryBl.GetObject
                            .GetTestersByDistance(address, license
                            ).Where(x =>
                                x.LicenseTypeTeaching.Any(y => y == license)).ToList();

                        void Action()
                        {
                            testerIdComboBox.ItemsSource = testers;
                        }

                        Dispatcher.BeginInvoke((Action) Action);
                    }
                    catch (Exception ex)
                    {
                        void Act()
                        {
                            ExceptionMessage.Show(ex.Message, ex.ToString());
                        }

                        Dispatcher.BeginInvoke((Action) Act);
                    }

                    void Action1()
                    {
                        //Set new Message
                        ClearAllMessages();
                        AddMessage("Please Select Tester.");

                        //Enable tester comBox and disable the rest
                        addressOfBeginningTestTextBox.IsEnabled = true;
                        traineeIdComboBox.IsEnabled = true;
                        licenseTypeComBox.IsEnabled = true;
                        testerIdComboBox.IsEnabled = true;
                        TimePickerTest.IsEnabled = false;
                        testTimeDatePicker.IsEnabled = false;
                        Save.IsEnabled = false;

                        ProgressRing.IsActive = false;

                        if (!testers.Any()) testerIdComboBox.IsEnabled = false;

                        //focus on testers
                        testerIdComboBox.Focus();
                    }

                    Dispatcher.BeginInvoke((Action) Action1);
                }).Start();

                //Update the license
                _test.LicenseType = (LicenseType) licenseTypeComBox.SelectedItem;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     When user Selects Tester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterIdComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //set the tester
                _test.TesterId = ((Tester) testerIdComboBox.SelectedItem).Id;

                //Blackout the Time Picker according to the Schedule
                SetSelectableDates(((Tester) testerIdComboBox.SelectedItem).Schedule);

                //Enable Date picker and disable the rest
                testTimeDatePicker.IsEnabled = true;
                TimePickerTest.IsEnabled = false;
                Save.IsEnabled = false;

                //Set new Message
                ClearAllMessages();
                AddMessage("Please Select Test Date an Time.");

                //Focus on next
                testTimeDatePicker.Focus();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     When user Selects Date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestTimeDatePicker_OnCalendarOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reset old selections
                TimePickerTest.ResetSelection();
                if (testTimeDatePicker.SelectedDate != null)
                {
                    var date = (DateTime) testTimeDatePicker.SelectedDate;
                    var tester = (Tester) testerIdComboBox.SelectedItem;
                    var hours = (bool[]) ((Tester) testerIdComboBox.SelectedItem).Schedule
                        .Days[(int) date.DayOfWeek].Hours.Clone();

                    //disable hours today that passed
                    if (date.DayOfYear == DateTime.Now.DayOfYear && date.Year == DateTime.Now.Year)
                        for (var i = DateTime.Now.Hour; i > 0; i--)
                            hours[i] = false;

                    //disable hours that the tester has a test already
                    var hourNum = new[]
                        {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};
                    foreach (var h in hourNum)
                        if (FactoryBl.GetObject.AllTests.Any(x =>
                            date.Year == x.TestTime.Year && date.DayOfYear == x.TestTime.DayOfYear &&
                            x.TestTime.Hour == h && x.TesterId == tester.Id))
                            hours[h] = false;

                    //Set The hours according to the schedule
                    TimePickerTest.HourToShow = hours;
                }

                //Enable Time Picker
                TimePickerTest.IsEnabled = true;
                Save.IsEnabled = false;

                //Set new Message
                ClearAllMessages();
                AddMessage("Please Select Test Time.");

                //Focus on Time Picker
                TimePickerTest.Focus();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     When user Selects Hour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimePicker_OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //Update the hour
                if (testTimeDatePicker.SelectedDate != null)
                {
                    var date = (DateTime) testTimeDatePicker.SelectedDate;
                    _test.TestTime = new DateTime(date.Year, date.Month, date.Day, TimePickerTest.SelectedHour, 0, 0);
                }

                Save.IsEnabled = true;
                ClearAllMessages();

                //Focus on button
                Save.Focus();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     When user Changes Address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressOfBeginningTestTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Reset all selections
                licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                testerIdComboBox.SelectedIndex = -1;
                licenseTypeComBox.SelectedIndex = -1;
                TimePickerTest.ResetSelection();
                testerIdComboBox.ItemsSource = null;
                licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;


                //Show Message
                ClearAllMessages();
                AddMessage("Please Select license.");

                //Disable all the controls
                testerIdComboBox.IsEnabled = false;
                TimePickerTest.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                Save.IsEnabled = false;
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     When user Sets Actual Test Time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActualTimePickerTestOnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //Update the time
                _test.ActualTestTime = new DateTime(_test.ActualTestTime.Year, _test.ActualTestTime.Month,
                    _test.ActualTestTime.Day, actualTimePickerTest.SelectedHour, 0, 0);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Blackout Days in the Test Time Picker
        /// </summary>
        /// <param name="schedule"></param>
        private void SetSelectableDates(WeekSchedule schedule)
        {
            try
            {
                testTimeDatePicker.DisplayDateStart = DateTime.Now.AddDays(-1);
                testTimeDatePicker.DisplayDateEnd = DateTime.Now.AddMonths(2);
                testTimeDatePicker.BlackoutDates.Clear();

                //Blackout all the dates in the past and in more that 2 month
                testTimeDatePicker.BlackoutDates.AddDatesInPast();
                testTimeDatePicker.BlackoutDates.Add(
                    new CalendarDateRange(DateTime.Now.AddMonths(2), DateTime.MaxValue));

                var date = DateTime.Now;

                //make an arr with days that the tester is available on
                var weekSchedule = new[] {false, false, false, false, false, false, false};
                foreach (var day in schedule.Days)
                    if (day.Hours.Any(x => x))
                        weekSchedule[(int) day.TheDay] = true;

                var tester = (Tester) testerIdComboBox.SelectedItem;
                var dateNow = DateTime.Today;
                var hourNmu = new[]
                    {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};

                //add the days of the 2 month in the calendar
                for (var i = 0; i < 64; i++)
                {
                    //the day need to be blacked out
                    if (!weekSchedule[(int) date.DayOfWeek] || hourNmu
                            .Where(z => tester.Schedule[date.DayOfWeek].Hours[z]).All(x =>
                                FactoryBl.GetObject.AllTests.Any(y =>
                                    y.TesterId == tester.Id && y.TestTime.Year == date.Year &&
                                    y.TestTime.DayOfYear == date.DayOfYear && y.TestTime.Hour == x)) ||
                        date.Year == DateTime.Now.Year && date.DayOfYear == DateTime.Now.DayOfYear &&
                        DateTime.Now.Hour >= schedule[DateTime.Now.DayOfWeek].MaxHourWorking())
                    {
                        //if today is already selected then move the selection to tomorrow
                        if (date.DayOfYear == dateNow.DayOfYear)
                        {
                            dateNow = dateNow.AddDays(1);
                            testTimeDatePicker.SelectedDate = _test.TestTime.AddDays(1);
                        }

                        //Add the days
                        testTimeDatePicker.BlackoutDates.Add(new CalendarDateRange(date));
                    }

                    date = date.AddDays(1);
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Notifications

        /// <summary>
        ///     Show Binding errors in Notification area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTest_OnError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) _errorMessage.Add(e.Error.Exception.Message);
            else _errorMessage.Remove(e.Error.Exception.Message);
            Errors.Text = "";
            foreach (var item in _errorMessage) Errors.Text += item + "\n";
        }

        /// <summary>
        ///     Add a message to the notification area
        /// </summary>
        /// <param name="message"></param>
        private void AddMessage(string message)
        {
            _notifications.Add(message);
            Errors.Text = "";
            foreach (var item in _notifications) Errors.Text += item + "\n";
        }

        /// <summary>
        ///     Clean all notifications
        /// </summary>
        private void ClearAllMessages()
        {
            _notifications.Clear();
            Errors.Text = "";
        }

        #endregion

        #region  Route

        /// <summary>
        ///     Set new Route for the test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetRouteButton_Click(object sender, RoutedEventArgs e)
        {
            //Add Message
            AddMessage("Adding Route to Test....");

            SetRouteButton.IsEnabled = false;
            //Get address
            var address = addressOfBeginningTestTextBox.Address;

            //Try to Make a route
            new Thread(() =>
            {
                try
                {
                    _test.SetRouteAndAddressToTest(address);
                }
                //Cast the Error to a more simple Message
                catch (GoogleAddressException ex)
                {
                    void Act()
                    {
                        switch (ex.ErrorCode)
                        {
                            case "CONNECTION_FAILURE":
                                ExceptionMessage.Show("There is no Internet Connection. Please try again later.",
                                    ex.Message);
                                break;
                            case "ADDRESS_FAILURE":
                                ExceptionMessage.Show(
                                    "There is a problem with the address. Please try another address.", ex.Message);
                                break;
                        }
                    }

                    Dispatcher.BeginInvoke((Action) Act);
                }

                void Action()
                {
                    //Update the show route
                    ShowRouteUrlButton.IsEnabled = _test.RouteUrl != null;

                    SetRouteButton.IsEnabled = true;
                    ClearAllMessages();
                }

                Dispatcher.BeginInvoke((Action) Action);
            }).Start();
        }

        /// <summary>
        ///     Show route on map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowRouteUrlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Routes.ShowUrlInChromeWindow(_test.RouteUrl);
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message);
            }
        }

        #endregion
    }
}