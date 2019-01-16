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

namespace PLWPF.Admin.ManageTrainee
{
    /// <summary>
    ///     Add or update trainee
    /// </summary>
    public partial class AddTrainee : MetroWindow
    {
        private readonly IBL _blimp = FactoryBl.GetObject;

        /// <summary>
        ///     all the exceptions
        /// </summary>
        private readonly List<string> _errorMessage = new List<string>();

        /// <summary>
        ///     collection to work with the license
        /// </summary>
        private readonly ObservableCollection<TrainingDetails> _licenses = new ObservableCollection<TrainingDetails>();

        /// <summary>
        ///     the trainee
        /// </summary>
        private readonly Trainee _trainee = new Trainee();

        /// <summary>
        ///     true if it is an update
        /// </summary>
        private readonly bool _update;

        /// <summary>
        ///     Add or update an trainee
        /// </summary>
        /// <param name="id">if it is an update then put the trainee id </param>
        public AddTrainee(uint id = 0)
        {
            InitializeComponent();

            //initialize as add
            if (id == 0)
            {
                DataContext = _trainee;
                _trainee.BirthDate = DateTime.Now.AddYears(-(int) Configuration.MinTraineeAge).AddDays(-1);
                birthDateDatePicker.DisplayDateEnd =
                    DateTime.Now.AddYears(-(int) Configuration.MinTraineeAge).AddDays(-1);
                Title = "Add New Trainee";
            }
            //initialize as update
            else
            {
                try
                {
                    birthDateDatePicker.DisplayDateEnd =
                        DateTime.Now.AddYears(-(int) Configuration.MinTraineeAge).AddDays(-1);

                    Title = "Update Trainee";
                    _trainee = _blimp.AllTrainees.First(x => x.Id == id);
                    DataContext = _trainee;
                    idTextBox.IsEnabled = false;
                    _update = true;
                    AddressTextBox.Address = _trainee.Address;
                }
                catch
                {
                    Close();
                }
            }

            BoxColumnGear.ItemsSource = Enum.GetValues(typeof(Gear));

            //set comBox source
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));

            //set the choose license 
            ChooseLicense.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ChooseLicense.SelectedItem = LicenseType.A;

            //initialize the license grid
            if (id != 0)
                foreach (var item in _trainee.LicenseTypeLearning)
                    _licenses.Add(item);
            LicenseDataGrid.ItemsSource = _licenses;

            //disable the Save button
            Save.IsEnabled = false;
        }

        /// <summary>
        ///     On click save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //update the license
                _trainee.LicenseTypeLearning = _licenses.ToList();

                //update or add the trainee
                if (_update)
                    _blimp.UpdateTrainee(_trainee);
                else
                    _blimp.AddTrainee(_trainee);
                Close();
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message, ex.ToString());
            }
        }

        /// <summary>
        ///     On Id text box change check the id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Save.IsEnabled = Tools.CheckID_IL(uint.Parse(idTextBox.Text));
            }
            catch
            {
                //let the user type only numbers
                if (idTextBox.Text != "")
                    idTextBox.Text = idTextBox.Text.Substring(0, idTextBox.Text.Length - 1);
                idTextBox.CaretIndex = idTextBox.Text.Length;
                Save.IsEnabled = false;
            }
        }

        /// <summary>
        ///     When an error occured  in the data binding
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

        #region License

        /// <summary>
        ///     On add license click, add a new license
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLicenseButton_Click(object sender, RoutedEventArgs e)
        {
            //if he is learning it already
            if (_licenses.Any(x => x.License == (LicenseType) ChooseLicense.SelectedItem))
                return;

            //Add the new license
            var number = NumberOfLessonsTextBox.Value;
            if (number != null)
                _licenses.Add(new TrainingDetails
                {
                    License = (LicenseType) ChooseLicense.SelectedItem,
                    NumberOfLessons = (int) number,
                    ReadyForTest = number > Configuration.MinLessons,
                    GearType = Gear.Automatic
                });
        }

        /// <summary>
        ///     On remove click, remove the license
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLicenseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _licenses.Remove(_licenses.First(x => x.License == (LicenseType) ChooseLicense.SelectedItem));
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Refresh the license grid after update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshLicense(object sender, EventArgs e)
        {
            try
            {
                LicenseDataGrid.Items.Refresh();
            }
            catch
            {
                // ignored
            }
        }

        #endregion
    }
}