using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.MainObjects;
using BE.Routes;
using BL;
using MahApps.Metro.Controls;

namespace PLWPF.Admin.ManageTester
{
    /// <summary>
    ///     Add or update tester
    /// </summary>
    public partial class AddTester : MetroWindow
    {
        private readonly IBL _blimp = FactoryBl.GetObject;

        //all the errors
        private readonly List<string> errorMessage = new List<string>();
        private ObservableCollection<LessonsAndType> licenses = new ObservableCollection<LessonsAndType>();

        private readonly Tester tester = new Tester();

        //if it is an update
        private readonly bool update;

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
                DataContext = tester;
                //set defualt falues
                tester.BirthDate = DateTime.Now.Date;
                tester.Schedule = new WeekSchedule();
                Title = "Add New Tester";
            }
            else
            {
                try
                {
                    update = true;
                    //find the existing tester
                    tester = _blimp.AllTesters.First(x => x.Id == id);
                    //bind it
                    DataContext = tester;
                    //disable field that cant be changed
                    idTextBox.IsEnabled = false;
                    AllWeek.IsEnabled = false;
                    AllWeek.IsChecked = false;
                    DayWeek.IsEnabled = true;
                    //set the address
                    AddressTextBox.Address = tester.Address != null ? tester.Address : null;

                    Title = "Update Tester";
                }
                catch
                {
                    Close();
                }

                AddressTextBox.TextChanged += AddressTextBox_TextChanged;
            }

            //set combox source
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            Chooselicense.ItemsSource = Enum.GetValues(typeof(LicenseType));

            //set day in week combox source
            var list = new List<DayOfWeek>();
            foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
                list.Add((DayOfWeek) item);
            DayWeek.ItemsSource = list.Take(5);
            DayWeek.SelectedItem = DayOfWeek.Sunday;

            //set hours combox
            var hours = new List<string>();
            for (var i = Configuration.MinStartHourWork; i < Configuration.MaxEndHourWork+1; i++) hours.Add(string.Format("{0:00}:00", i));
            ChooseHours.ItemsSource = hours;
            DayWeek.SelectedItem = "All Days";

            //initilse licnse type learning
            if (tester.LicenseTypeTeaching == null) tester.LicenseTypeTeaching = new List<LicenseType>();
            if (update)
                foreach (var item in tester.LicenseTypeTeaching)
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
                if (Tools.CheckID_IL(uint.Parse(idTextBox.Text)))
                    Save.IsEnabled = true;
                else
                    Save.IsEnabled = false;
            }
            catch
            {
                idTextBox.Text = "";
                Save.IsEnabled = false;
            }
        }

        /// <summary>
        ///     update address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                tester.Address = AddressTextBox.Address;
            }
            catch
            {
                AddressTextBox.Address = null;
            }
        }
  
        /// <summary>
        ///     when license selection changed update license list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chooselicense_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tester.LicenseTypeTeaching = new List<LicenseType>();
            foreach (var item in Chooselicense.SelectedItems) tester.LicenseTypeTeaching.Add((LicenseType) item);
        }

        #region Schedule

        /// <summary>
        ///     disable day of week combox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllWeek_Checked(object sender, RoutedEventArgs e)
        {
            DayWeek.IsEnabled = false;
        }

        /// <summary>
        ///     enable day of week combox
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
                foreach (var day in tester.Schedule.days)
                {
                    day.ClearHours();
                    foreach (var hour in ChooseHours.SelectedItems)
                        day.Hours[int.Parse(((string)hour).Substring(0, 2))] = true;
                }
            }
            //update one day hours
            else
            {
                var day = tester.Schedule[(DayOfWeek)DayWeek.SelectedItem];
                day.ClearHours();
                foreach (var hour in ChooseHours.SelectedItems)
                    day.Hours[int.Parse(((string)hour).Substring(0, 2))] = true;
            }
        }

        /// <summary>
        ///     update Day in week selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var day = tester.Schedule[(DayOfWeek)DayWeek.SelectedItem];
            var i = 0;
            var list = new List<string>();
            foreach (var hour in day.Hours)
            {
                if (hour) list.Add(string.Format("{0:00}:00", i));
                i++;
            }

            ChooseHours.UnselectAll();
            foreach (var item in list) ChooseHours.SelectedItems.Add(item);
        }


        #endregion

        /// <summary>
        ///     When an error is thowed in data binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) errorMessage.Add(e.Error.Exception.Message);
            else errorMessage.Remove(e.Error.Exception.Message);
            ErrorMessage.Text = "";
            foreach (var item in errorMessage) ErrorMessage.Text += item + "\n";
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
                tester.Address = AddressTextBox.Address;
                if (update)
                    _blimp.UpdateTester(tester);
                else
                    _blimp.AddTester(tester);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}