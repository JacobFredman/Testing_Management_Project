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

namespace PLWPF.Admin.ManageTest
{
    /// <summary>
    /// Interaction logic for EditTest.xaml
    /// </summary>
    public partial class EditTest : Window
    {
        private readonly List<string> errorMessage = new List<string>();
        private readonly List<string> notifications = new List<string>();


        private Test _test;
        public EditTest(string id=null)
        {
            InitializeComponent();
            traineeIdComboBox.ItemsSource = BL.FactoryBl.GetObject.AllTrainees.Where(x=>x.LicenseTypeLearning.Any(y=>y.ReadyForTest)).ToList();

            if (id == null)
            {
                _test = new Test();
                actualTestTimeDatePicker.IsEnabled = false;
                TestResolutsGroupBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                AddShowMessage("Please Select Trainee");
                addressOfBeginningTestTextBox.TextChanged += AddressOfBeginningTestTextBox_TextChanged;
                Title = "Set New Test";
                _test.TestTime=DateTime.Now;
                Save.IsEnabled = false;

            }
            else
            {
                _test = BL.FactoryBl.GetObject.AllTests.First(x => x.Id == id);
                testerIdComboBox.SelectionChanged -= TesterIdComboBox_OnSelectionChanged;
                traineeIdComboBox.SelectionChanged -= TraineeIdComboBox_SelectionChanged;
                licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                testTimeDatePicker.SelectedDateChanged -= TestTimeDatePicker_SelectedDateChanged;
                TimePickerTest.SelectionChanged -= TimePicker_OnSelectionChanged;
                traineeIdComboBox.ItemsSource = new List<Trainee>() { BL.FactoryBl.GetObject.AllTrainees.First(x => x.Id == _test.TraineeId) };
                traineeIdComboBox.SelectedIndex = 0;
                testerIdComboBox.ItemsSource= new List<Tester>() { BL.FactoryBl.GetObject.AllTesters.First(x => x.Id == _test.TesterId) };
                testerIdComboBox.SelectedIndex = 0;
                licenseTypeComBox.ItemsSource= new List<LicenseType>() { _test.LicenseType };
                licenseTypeComBox.SelectedIndex = 0;
                 TimePickerTest.SelectedHour = _test.TestTime.Hour;
                Save.Content = "Update";
                Title = "Update Test";
                traineeIdComboBox.IsEnabled = false;
                testerIdComboBox.IsEnabled = false;
                licenseTypeComBox.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                Save.IsEnabled = true;
                if (_test.Criteria ==null||_test.Criteria.Count()<10)
                     _test.Criteria = Configuration.Criterions.Select(x => new Criterion(x, false)).ToList();
                _test.ActualTestTime=new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,0,0);
                actualTestTimeDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue,
                    DateTime.Now.AddDays(-5)));
                actualTimePickerTest.SelectedHour = _test.ActualTestTime.Hour;
                actualTimePickerTest.SelectionChanged+= ActualTimePickerTestOnSelectionChanged;
                addressOfBeginningTestTextBox.Address = _test.AddressOfBeginningTest;

            }

            TimePickerTest.IsEnabled = false;
            DataContext = _test;
            if (_test.RouteUrl == null)
            {
                ShowRouteUrlButton.IsEnabled = false;
            }
   

        }

        private void ActualTimePickerTestOnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                _test.ActualTestTime = new DateTime(_test.ActualTestTime.Year, _test.ActualTestTime.Month,
                    _test.ActualTestTime.Day, actualTimePickerTest.SelectedHour, 0, 0);
            }
            catch { }
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
            try
            {
                testerIdComboBox.SelectedValue = -1;

                licenseTypeComBox.SelectionChanged -= LicenseTypeComBox_OnSelectionChanged;
                licenseTypeComBox.SelectedValue = -1;
                licenseTypeComBox.ItemsSource = ((Trainee) traineeIdComboBox.SelectedItem).LicenseTypeLearning
                    .Where(y => y.ReadyForTest)
                    .Select(x => x.License).ToList();
                licenseTypeComBox.SelectionChanged += LicenseTypeComBox_OnSelectionChanged;

                testerIdComboBox.ItemsSource = null;


                _test.TraineeId = ((Trainee) traineeIdComboBox.SelectedItem).Id;
                addressOfBeginningTestTextBox.Address = ((Trainee) traineeIdComboBox.SelectedItem).Address;

                licenseTypeComBox.IsEnabled = true;
                testerIdComboBox.IsEditable = false;
                TimePickerTest.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                Save.IsEnabled = false;
                AddShowMessage("Please Select license.");
                licenseTypeComBox.Focus();
            }
            catch { }
        }

        private void LicenseTypeComBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WaitTraineeLabel.Visibility = Visibility.Visible;
                var address = addressOfBeginningTestTextBox.Address;
                var license = (LicenseType) licenseTypeComBox.SelectedItem;
                _test.LicenseType = license;
                (new Thread(() =>
                {
                    try
                    {
                        var testers = BL.FactoryBl.GetObject
                            .GetTestersByDistance(address, license
                            ).Where(x =>
                                x.LicenseTypeTeaching.Any(y => y == license)).ToList();
                        Action action = () => { testerIdComboBox.ItemsSource = testers; };
                        Dispatcher.BeginInvoke(action);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    Action action1 = () =>
                    {
                        WaitTraineeLabel.Visibility = Visibility.Hidden;
                        RemoveShowMessage();
                        AddShowMessage("Please Select Tester.");
                        testerIdComboBox.IsEnabled = true;
                        TimePickerTest.IsEnabled = false;
                        testTimeDatePicker.IsEnabled = false;
                        Save.IsEnabled = false;
                        testerIdComboBox.Focus();
                    };
                    Dispatcher.BeginInvoke(action1);

                })).Start();
                _test.LicenseType = (LicenseType) licenseTypeComBox.SelectedItem;

         
            }
            catch { }
        }

        private void TesterIdComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _test.TesterId = ((Tester) testerIdComboBox.SelectedItem).Id;
                SetSelectableDates(((Tester) testerIdComboBox.SelectedItem).Schedule);
                testTimeDatePicker.IsEnabled = true;
                TimePickerTest.IsEnabled = false;
                Save.IsEnabled = false;
                RemoveShowMessage();
                AddShowMessage("Please Select Test Date.");
                testTimeDatePicker.Focus();
            }
            catch { }
        }

        private void TestTimeDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TimePickerTest.HourToShow = ((Tester)testerIdComboBox.SelectedItem).Schedule
                    .days[(int)((DateTime)testTimeDatePicker.SelectedDate).DayOfWeek].Hours;
                TimePickerTest.IsEnabled = true;
                Save.IsEnabled = false;
                RemoveShowMessage();
                AddShowMessage("Please Select Test Time.");
                TimePickerTest.Focus();
            }
            catch{}
        }


        private void TimePicker_OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                var date = (DateTime) testTimeDatePicker.SelectedDate;
                _test.TestTime = new DateTime(date.Year, date.Month, date.Day, TimePickerTest.SelectedHour, 0, 0);
                Save.IsEnabled = true;
                RemoveShowMessage();
                Save.Focus();
            }
            catch { }
        }

        private void SetSelectableDates(WeekSchedule schedule)
        {
            try
            {
                testTimeDatePicker.BlackoutDates.Clear();
                testTimeDatePicker.BlackoutDates.AddDatesInPast();
                testTimeDatePicker.BlackoutDates.Add(
                    new CalendarDateRange(DateTime.Now.AddMonths(2), DateTime.MaxValue));
                var date = DateTime.Now;
                bool[] weekSchedule = new bool[7] {false, false, false, false, false, false, false};
                foreach (var day in schedule.days)
                {
                    if (day.Hours.Any(x => x == true))
                        weekSchedule[(int) day.TheDay] = true;
                }

                for (int i = 0; i < 64; i++)
                {
                    if (!weekSchedule[(int) (date.DayOfWeek)])
                        testTimeDatePicker.BlackoutDates.Add(new CalendarDateRange(date));
                    date = date.AddDays(1);
                }
            }
            catch { }
        }

   
        private void AddressOfBeginningTestTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _test.AddressOfBeginningTest = addressOfBeginningTestTextBox.Address;
                testerIdComboBox.SelectedIndex = -1;
                licenseTypeComBox.SelectedIndex = -1;
                testerIdComboBox.IsEditable = false;
                TimePickerTest.IsEnabled = false;
                testTimeDatePicker.IsEnabled = false;
                Save.IsEnabled = false;
            }
            catch { }
        }

        private void EditTest_OnError(object sender, ValidationErrorEventArgs e)
        {
           
                if (e.Action == ValidationErrorEventAction.Added) errorMessage.Add(e.Error.Exception.Message);
                else errorMessage.Remove(e.Error.Exception.Message);
                Errors.Text = "";
                foreach (var item in errorMessage) Errors.Text += item + "\n";
            
        }

        private void AddShowMessage(string message)
        {
            notifications.Add(message);
            Errors.Text = "";
            foreach (var item in notifications) Errors.Text += item + "\n";
        }

        private void RemoveShowMessage()
        {
            notifications.Clear();
            Errors.Text = "";
        }

        private void SetRouteButton_Click(object sender, RoutedEventArgs e)
        {
            AddShowMessage("Adding Route to Test....");
            SetRouteButton.IsEnabled = false;
            var address = addressOfBeginningTestTextBox.Address;
            (new Thread(() =>
            {
                try
                {
                    _test.SetRouteAndAddressToTest(address);
                }
                catch (GoogleAddressException ex)
                {
                    if (ex.ErrorCode == "CONNECTION_FAILURE")
                    {
                        MessageBox.Show("There is no Internet Connection. Please try again later.\nDetails: " +
                                        ex.Message);
                    }
                    else if (ex.ErrorCode == "ADDRESS_FAILURE")
                    {
                        MessageBox.Show("There is a problem with the address. Please try another address.\nDetails: " +
                                        ex.Message);
                    }
                }

                Action action = () =>
                {
                    if (_test.RouteUrl != null)
                    {
                        ShowRouteUrlButton.IsEnabled = true;
                    }
                    else
                    {
                        ShowRouteUrlButton.IsEnabled = false;
                    }

                    SetRouteButton.IsEnabled = true;
                    RemoveShowMessage();
                };
                Dispatcher.BeginInvoke(action);

            })).Start();


        }

        private void ShowRouteUrlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.Routes.ShowUrlInChromeWindow(_test.RouteUrl);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
    }
}
