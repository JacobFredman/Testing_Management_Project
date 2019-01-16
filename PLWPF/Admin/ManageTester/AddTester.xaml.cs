using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Nofitications;

namespace PLWPF.Admin.ManageTester
{
    /// <summary>
    ///     Add or update tester
    /// </summary>
    public partial class AddTester : MetroWindow
    {
        private readonly IBL _blimp = FactoryBl.GetObject;

        /// <summary>
        /// all the errors
        /// </summary>
        private readonly List<string> _errorMessage = new List<string>();

        /// <summary>
        /// the tester
        /// </summary>
        private readonly Tester _tester = new Tester();

        /// <summary>
        /// if it is an update
        /// </summary>
        private readonly bool _update;

        /// <summary>
        ///     Add tester window
        /// </summary>
        /// <param name="id">tester id if it is an update</param>
        public AddTester(uint id = 0)
        {
            InitializeComponent();


            if (id == 0)
            {
                //bind the new tester to the window
                DataContext = _tester;
                //set default values
                _tester.BirthDate = DateTime.Now.AddYears(-(int)Configuration.MinTesterAge).AddDays(-1);
                birthDateDatePicker.DisplayDateEnd= DateTime.Now.AddYears(-(int)Configuration.MinTesterAge).AddDays(-1);
                _tester.Schedule = new WeekSchedule();
                Title = "Add New Tester";
            }
            else
            {
                try
                {
                    birthDateDatePicker.DisplayDateEnd = DateTime.Now.AddYears(-(int)Configuration.MinTesterAge).AddDays(-1);
                    _update = true;
                    //find the existing tester
                    _tester = _blimp.AllTesters.First(x => x.Id == id);
                    //bind it
                    DataContext = _tester;
                    //disable field that cant be changed
                    idTextBox.IsEnabled = false;
                    AllWeek.IsEnabled = false;
                    AllWeek.IsChecked = false;
                    DayWeek.IsEnabled = true;
                    //set the address
                    AddressTextBox.Address = _tester.Address;

                    Title = "Update Tester";
                }
                catch
                {
                    Close();
                }

            }

            //set comBox source
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            Chooselicense.ItemsSource = Enum.GetValues(typeof(LicenseType));

            //set day in week comBox source
            var list = new List<DayOfWeek>();
            foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
                list.Add((DayOfWeek) item);
            DayWeek.ItemsSource = list.Take(5);
            DayWeek.SelectedItem = DayOfWeek.Sunday;

            //set hours comBox
            var hours = new List<string>();
            for (var i = Configuration.MinStartHourWork; i < Configuration.MaxEndHourWork + 1; i++)
                hours.Add($"{i:00}:00");
            ChooseHours.ItemsSource = hours;
            DayWeek.SelectedItem = "All Days";

            //initialise license type learning
            if (_tester.LicenseTypeTeaching == null) _tester.LicenseTypeTeaching = new List<LicenseType>();
            if (_update)
                foreach (var item in _tester.LicenseTypeTeaching)
                    Chooselicense.SelectedItems.Add(item);
        }

        /// <summary>
        ///     on id changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if id is correct the enable save
            try
            {
                Save.IsEnabled = Tools.CheckID_IL(uint.Parse(idTextBox.Text));
            }
            catch
            {
                if(idTextBox.Text!="")
                   idTextBox.Text = idTextBox.Text.Substring(0, idTextBox.Text.Length-1);
                idTextBox.CaretIndex = idTextBox.Text.Length;
                Save.IsEnabled = false;
            }
        }

        /// <summary>
        ///     when license selection changed update license list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseLicense_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _tester.LicenseTypeTeaching = new List<LicenseType>();
            foreach (var item in Chooselicense.SelectedItems) _tester.LicenseTypeTeaching.Add((LicenseType) item);
        }

        /// <summary>
        ///     When an error is trowed in data binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) _errorMessage.Add(e.Error.Exception.Message);
            else _errorMessage.Remove(e.Error.Exception.Message);
            ErrorMessage.Text = "";
            foreach (var item in _errorMessage) ErrorMessage.Text += item + "\n";
        }

        /// <summary>
        ///     On save clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //update or save the tester
            try
            {
                _tester.Address = AddressTextBox.Address;
                if (_update)
                    _blimp.UpdateTester(_tester);
                else
                    _blimp.AddTester(_tester);
                Close();
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message, ex.ToString());
            }
        }

        #region Schedule

        /// <summary>
        ///     disable day of week comBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllWeek_Checked(object sender, RoutedEventArgs e)
        {
            DayWeek.IsEnabled = false;
        }

        /// <summary>
        ///     enable day of week comBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllWeek_Unchecked(object sender, RoutedEventArgs e)
        {
            DayWeek.IsEnabled = true;
        }

        /// <summary>
        ///     On Hours selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseHours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update  hours in all days in week
            if (AllWeek.IsChecked == true)
            {
                foreach (var day in _tester.Schedule.Days)
                {
                    day.ClearHours();
                    foreach (var hour in ChooseHours.SelectedItems)
                        day.Hours[int.Parse(((string) hour).Substring(0, 2))] = true;
                }
            }
            //update one day hours
            else
            {
                var day = _tester.Schedule[(DayOfWeek) DayWeek.SelectedItem];
                day.ClearHours();
                foreach (var hour in ChooseHours.SelectedItems)
                    day.Hours[int.Parse(((string) hour).Substring(0, 2))] = true;
            }
        }

        /// <summary>
        ///     update Day in week selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var day = _tester.Schedule[(DayOfWeek) DayWeek.SelectedItem];
            var i = 0;
            var list = new List<string>();
            foreach (var hour in day.Hours)
            {
                if (hour) list.Add($"{i:00}:00");
                i++;
            }

            ChooseHours.UnselectAll();
            foreach (var item in list) ChooseHours.SelectedItems.Add(item);
        }

        #endregion

    
    }
}