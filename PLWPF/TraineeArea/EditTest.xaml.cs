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
        /// <summary>
        /// Error messages
        /// </summary>
        private readonly List<string> _errorMessage = new List<string>();

        /// <summary>
        /// Notification messages
        /// </summary>
        private readonly List<string> _notifications = new List<string>();

        /// <summary>
        /// the test
        /// </summary>
        private readonly Test _test;

        /// <summary>
        /// The tester
        /// </summary>
        private Tester _tester;

        /// <summary>
        /// Edit a test
        /// </summary>
        /// <param name="trainee"></param>
        public EditTest(Trainee trainee)
        {
            InitializeComponent();
            try
            {
                //set title
                Title = "Set New Test";

                _test = new Test();

                //check if the trainee has a license to do a test
                if (!trainee.LicenseTypeLearning.Any(x => x.ReadyForTest))
                    AddMessage("the trainee are not ready for test");

                //set license item source
                licenseTypeComBox.ItemsSource = trainee.LicenseTypeLearning.Where(y => y.ReadyForTest)
                    .Select(x => x.License).ToList();

                //set the source of the trainees
                idTextBox.Text = "Id: " + trainee.Id + " Name: " + trainee.FirstName + " " + trainee.LastName;

                //set default date
                _test.TestTime = DateTime.Now;

                //set trainee id
                _test.TraineeId = trainee.Id;

                //set data context
                DataContext = _test;

                //set address
                addressOfBeginningTestTextBox.Address = trainee.Address;
               
                //add events
                addressOfBeginningTestTextBox.TextChanged += AddressOfBeginningTestTextBox_TextChanged;
                licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;

                //enable the license and disable the other
                licenseTypeComBox.IsEnabled = true;
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
        /// Save the Test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
         
                //Check address
                if (addressOfBeginningTestTextBox.Address.ToString() == "")
                    ExceptionMessage.Show("Please Select an Address for the test");

                //Update address
                _test.AddressOfBeginningTest = addressOfBeginningTestTextBox.Address;
                AddMessage("Saving test...");

                //try to add route and save
               (new Thread(() =>
               {
                   try
                   {
                       _test.SetRouteAndAddressToTest(_test.AddressOfBeginningTest);
                   }
                   catch
                   {
                       //Do nothing
                   }
                   try { 
                  
                       //Add test
                       FactoryBl.GetObject.AddTest(_test);

                       void Act()
                       {
                           Close();
                       }

                       Dispatcher.BeginInvoke((Action) Act);
                   }
                   catch (Exception ex)
                   {
                       void Act1()
                       {
                           ExceptionMessage.Show(ex.Message, ex.ToString());
                           ClearAllMessages();
                       }

                       Dispatcher.BeginInvoke((Action) Act1);
                   }              
               })).Start();    
        }

        #region Details
        /// <summary>
        /// When user Selects License type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LicenseTypeComBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if the selection is the same
                if ((LicenseType) licenseTypeComBox.SelectedItem == _test.LicenseType)
                    return;

                //Show wait message
                ClearAllMessages();
                AddMessage("Searching for Tester. Please wait.");

                //enable and disable controls
                licenseTypeComBox.IsEnabled = false;
                addressOfBeginningTestTextBox.IsEnabled = false;
                ProgressRing.IsActive = true;
                testTimeDatePicker.IsEnabled = false;
                TimePickerTest.IsEnabled = false;
                TimePickerTest.ResetSelection();

                //Get the data
                var address = addressOfBeginningTestTextBox.Address;
                var license = (LicenseType) licenseTypeComBox.SelectedItem;
                _test.LicenseType = license;

                //todo: Background worker
                //find all available testers and choose the first
                new Thread(() =>
                {
                    try
                    {
                        //get the tester
                        IEnumerable<Tester> testers = FactoryBl.GetObject
                            .GetTestersByDistance(address, license
                            ).Where(x =>
                                x.LicenseTypeTeaching.Any(y => y == license)).ToList();
                        _test.TesterId = testers.First().Id;
                        _tester = testers.First();

                        void Action()
                        {
                            //Set new Message
                            ClearAllMessages();
                            AddMessage("Please Select Test Date.");

                            //Blackout the Time Picker according to the Schedule
                            SetSelectableDates(testers.First().Schedule);

                            //Enable tester comBox and disable the rest
                            addressOfBeginningTestTextBox.IsEnabled = true;
                            licenseTypeComBox.IsEnabled = true;
                            testTimeDatePicker.IsEnabled = true;
                            TimePickerTest.IsEnabled = false;
                            Save.IsEnabled = false;
                            ProgressRing.IsActive = false;

                            testTimeDatePicker.Focus();
                        }

                        Dispatcher.BeginInvoke((Action) Action);
                    }
                    catch (Exception ex)
                    {
                        void Act()
                        {
                            ProgressRing.IsActive = false;
                            licenseTypeComBox.IsEnabled = true;

                            ExceptionMessage.Show("Sorry We Couldn't Find A Tester.",ex.Message);
                        }
                        Dispatcher.BeginInvoke((Action) Act);
                    }

                }).Start();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// When user Selects Date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestTimeDatePicker_OnCalendarOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reset old selections
                TimePickerTest.ResetSelection();

                var date = ((DateTime) testTimeDatePicker.SelectedDate);
                var hours = (bool[]) _tester.Schedule
                    .Days[(int) date.DayOfWeek].Hours.Clone();

                //disable hour that happened
                if (date.DayOfYear == DateTime.Now.DayOfYear && date.Year == DateTime.Now.Year)
                    for (int i = DateTime.Now.Hour; i > 0; i--)
                        hours[i] = false;

                //disable all hours that the tester has a test already
                var hourNum = new int[]
                    {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};
                foreach (var h in hourNum)
                {
                    if (FactoryBl.GetObject.AllTests.Any(x =>
                        date.Year == x.TestTime.Year && date.DayOfYear == x.TestTime.DayOfYear &&
                        x.TestTime.Hour == h && x.TesterId == _tester.Id))
                    {
                        hours[h] = false;
                    }
                }

                //Set The hours according to the schedule
                TimePickerTest.HourToShow = hours;

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
        /// When user Selects Hour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// When user Changes Address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressOfBeginningTestTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Update the address
                _test.AddressOfBeginningTest = addressOfBeginningTestTextBox.Address;

                //Reset all selections
                licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                licenseTypeComBox.SelectedIndex = -1;
                TimePickerTest.ResetSelection();
                licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;

                //Show Message
                ClearAllMessages();
                AddMessage("Please Select license.");

                //Disable all the controls
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
        /// Blackout Days in the Test Time Picker
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
                var weekSchedule = new [] {false, false, false, false, false, false, false};
                foreach (var day in schedule.Days)
                    if (day.Hours.Any(x => x))
                        weekSchedule[(int) day.TheDay] = true;

                var dateNow = DateTime.Today;
                var hourNmu = new []
                    {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};

                //add the days of the 2 month in the calendar
                for (var i = 0; i < 64; i++)
                {
                    //the day need to be blacked out
                    if (!weekSchedule[(int) date.DayOfWeek] || hourNmu
                            .Where(z => _tester.Schedule[date.DayOfWeek].Hours[z]).All(x =>
                                FactoryBl.GetObject.AllTests.Any(y =>
                                    y.TesterId == _tester.Id && y.TestTime.Year == date.Year &&
                                    y.TestTime.DayOfYear == date.DayOfYear && y.TestTime.Hour == x)) ||
                        date.Year == DateTime.Now.Year && date.DayOfYear == DateTime.Now.DayOfYear &&
                        DateTime.Now.Hour > schedule[DateTime.Now.DayOfWeek].MaxHourWorking())
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
        /// Show Binding errors in Notification area
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
        /// Add a message to the notification area
        /// </summary>
        /// <param name="message"></param>
        private void AddMessage(string message)
        {
            _notifications.Add(message);
            Errors.Text = "";
            foreach (var item in _notifications) Errors.Text += item + "\n";
        }

        /// <summary>
        /// Clean all notifications
        /// </summary>
        private void ClearAllMessages()
        {
            _notifications.Clear();
            Errors.Text = "";
        }

        #endregion 
    }
}