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

namespace PLWPF.TraineeArea
{
    /// <summary>
    ///     Interaction logic for EditTest.xaml
    /// </summary>
    public partial class EditTest : MetroWindow
    {
        private readonly List<string> _errorMessage = new List<string>();

        private readonly List<string> _notifications = new List<string>();

        //the test 
        private readonly Test _test;

        public EditTest(Trainee trainee)
        {
            InitializeComponent();

            //If the window adds a new test
           
                _test = new Test();

                //disable the relevant controls
                idTextBox.IsEnabled = false;
                //actualTestTimeDatePicker.IsEnabled = false;
                //actualTimePickerTest.IsEnabled = false;
                //TestResolutsGroupBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                TimePickerTest.IsEnabled = false;

                //set the source of the trainees
                if (trainee != null)
                    idTextBox.Text = "Id: " + trainee.Id + " Name: " + trainee.FirstName+" " + trainee.LastName;
                //traineeIdComboBox.ItemsSource = FactoryBl.GetObject.AllTrainees
                 //   .Where(x => x.LicenseTypeLearning.Any(y => y.ReadyForTest)).ToList();

                AddMessage("Please Select Trainee");
                //add even for uc
                addressOfBeginningTestTextBox.TextChanged += AddressOfBeginningTestTextBox_TextChanged;

                //set default date
                _test.TestTime = DateTime.Now;
                Save.IsEnabled = false;

                //set title
                Title = "Set New Test";










                try
                {
                    //Clean License and Tester ComBox
                    licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                    testerIdComboBox.SelectedValue = -1;
                    licenseTypeComBox.SelectedValue = -1;

                    if (!trainee.LicenseTypeLearning.Any(x => x.ReadyForTest))
                        AddMessage("the trainee are not ready for test");

                    licenseTypeComBox.ItemsSource = trainee.LicenseTypeLearning.Where(y => y.ReadyForTest)
                        .Select(x => x.License).ToList();

                licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;
                    testerIdComboBox.ItemsSource = null;
                ////////////////////////////////////////////////////////////////
                    ////Get All the Trainees that are ready for test
                    _test.TraineeId = trainee.Id;
                    addressOfBeginningTestTextBox.Address = trainee.Address;

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















            //set data context
            DataContext = _test;

            //set the url button
            if (_test.RouteUrl == null) ShowRouteUrlButton.IsEnabled = false;
        }

        //Save the Test
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check address
                if (addressOfBeginningTestTextBox.Address.ToString() == "")
                    throw new Exception("Please Select an Address for the test");
                //Update address
                _test.AddressOfBeginningTest = addressOfBeginningTestTextBox.Address;

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

        //When user Selects Trainee
        //private void TraineeIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        //Clean License and Tester ComBox
        //        licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
        //        testerIdComboBox.SelectedValue = -1;
        //        licenseTypeComBox.SelectedValue = -1;
        //        licenseTypeComBox.ItemsSource = ((Trainee) traineeIdComboBox.SelectedItem).LicenseTypeLearning
        //            .Where(y => y.ReadyForTest)
        //            .Select(x => x.License).ToList();
        //        licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;
        //        testerIdComboBox.ItemsSource = null;

        //        //Get All the Trainees that are ready for test
        //        _test.TraineeId = ((Trainee) traineeIdComboBox.SelectedItem).Id;
        //        addressOfBeginningTestTextBox.Address = ((Trainee) traineeIdComboBox.SelectedItem).Address;

        //        //enable the license and disable the other
        //        licenseTypeComBox.IsEnabled = true;
        //        testerIdComboBox.IsEditable = false;
        //        TimePickerTest.IsEnabled = false;
        //        testTimeDatePicker.IsEnabled = false;
        //        Save.IsEnabled = false;

        //        //set new Message
        //        ClearAllMessages();
        //        AddMessage("Please Select license.");

        //        //Focus on license
        //        licenseTypeComBox.Focus();
        //    }
        //    catch
        //    {
        //        // ignored
        //    }
        //}

        //When user Selects License type
        private void LicenseTypeComBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Show wait message
                ClearAllMessages();
                AddMessage("Searching for Tester. Please wait.");
                licenseTypeComBox.IsEnabled = false;
                //traineeIdComboBox.IsEnabled = false;
                addressOfBeginningTestTextBox.IsEnabled = false;
                ProgressRing.IsActive = true;


                try
                {
                    testerIdComboBox.SelectedIndex = -1;
                }
                catch { }

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
                        //traineeIdComboBox.IsEnabled = true;
                        licenseTypeComBox.IsEnabled = true;
                        testerIdComboBox.IsEnabled = true;
                        TimePickerTest.IsEnabled = false;
                        testTimeDatePicker.IsEnabled = false;
                        Save.IsEnabled = false;

                        ProgressRing.IsActive = false;

                        if (!testers.Any())
                        {
                            testerIdComboBox.IsEnabled = false;
                        }

                        //focus oon testers
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

        //When user Selects Tester
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

        //When user Selects Date
        private void TestTimeDatePicker_OnCalendarOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reset old selections
                TimePickerTest.ResetSelection();

                //Set The hours according to the schedule
                TimePickerTest.HourToShow = ((Tester) testerIdComboBox.SelectedItem).Schedule
                    .days[(int) ((DateTime) testTimeDatePicker.SelectedDate).DayOfWeek].Hours;

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

        //When user Selects Hour
        private void TimePicker_OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //Update the hour
                var date = (DateTime) testTimeDatePicker.SelectedDate;
                _test.TestTime = new DateTime(date.Year, date.Month, date.Day, TimePickerTest.SelectedHour, 0, 0);
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

        //When user Changes Address
        private void AddressOfBeginningTestTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Update the address
                _test.AddressOfBeginningTest = addressOfBeginningTestTextBox.Address;

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

        //When user Sets Actual Test Time
        //private void ActualTimePickerTestOnSelectionChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Update the time
        //        _test.ActualTestTime = new DateTime(_test.ActualTestTime.Year, _test.ActualTestTime.Month,
        //            _test.ActualTestTime.Day, actualTimePickerTest.SelectedHour, 0, 0);
        //    }
        //    catch
        //    {
        //        // ignored
        //    }
        //}

        //Blackout Days in the Test Time Picker
        private void SetSelectableDates(WeekSchedule schedule)
        {
            try
            {
                testTimeDatePicker.DisplayDateStart=DateTime.Now.AddDays(-1);
                testTimeDatePicker.DisplayDateEnd = DateTime.Now.AddMonths(2);
                testTimeDatePicker.BlackoutDates.Clear();

                //Blackout all the dates in the past and in more that 2 month
                testTimeDatePicker.BlackoutDates.AddDatesInPast();
                testTimeDatePicker.BlackoutDates.Add(
                    new CalendarDateRange(DateTime.Now.AddMonths(2), DateTime.MaxValue));

                var date = DateTime.Now;

                //make an arr with days that the tester is available on
                var weekSchedule = new bool[7] {false, false, false, false, false, false, false};
                foreach (var day in schedule.days)
                    if (day.Hours.Any(x => x))
                        weekSchedule[(int) day.TheDay] = true;


                var dateNow = DateTime.Today;

                //add the days of the 2 month in the calendar
                for (var i = 0; i < 64; i++)
                {
                    if (!weekSchedule[(int) date.DayOfWeek])
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

        //Show Binding errors in Notification area
        private void EditTest_OnError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) _errorMessage.Add(e.Error.Exception.Message);
            else _errorMessage.Remove(e.Error.Exception.Message);
            Errors.Text = "";
            foreach (var item in _errorMessage) Errors.Text += item + "\n";
        }

        //Add a message to the notification area
        private void AddMessage(string message)
        {
            _notifications.Add(message);
            Errors.Text = "";
            foreach (var item in _notifications) Errors.Text += item + "\n";
        }

        //Clean all notifications
        private void ClearAllMessages()
        {
            _notifications.Clear();
            Errors.Text = "";
        }

        #endregion

        #region  Route

        //Set new Route for the test
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
                    if (_test.RouteUrl != null)
                        ShowRouteUrlButton.IsEnabled = true;
                    else
                        ShowRouteUrlButton.IsEnabled = false;

                    //Update address
                    addressOfBeginningTestTextBox.Address = _test.AddressOfBeginningTest;

                    SetRouteButton.IsEnabled = true;
                    ClearAllMessages();
                }

                Dispatcher.BeginInvoke((Action) Action);
            }).Start();
        }

        //Show route on map
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