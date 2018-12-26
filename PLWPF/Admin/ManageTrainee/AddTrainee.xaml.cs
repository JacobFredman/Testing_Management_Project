﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.MainObjects;
using BL;

namespace PLWPF.Admin.ManageTrainee
{
    /// <summary>
    ///     Add or update trainee
    /// </summary>
    public partial class AddTrainee : Window
    {
        private readonly IBL _blimp = FactoryBl.GetObject;

        //all the exceptions
        private readonly List<string> errorMessage = new List<string>();

        //collection to work with the license
        private readonly ObservableCollection<LessonsAndType> licenses = new ObservableCollection<LessonsAndType>();

        private readonly Trainee trainee = new Trainee();

        //true if it is an update
        private readonly bool update;

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
                DataContext = trainee;
                trainee.BirthDate = DateTime.Now.Date;
                Title = "Add New Trainee";
            }
            //initialize as update
            else
            {
                try
                {
                    Title = "Update Trainee";
                    trainee = _blimp.AllTrainees.First(x => x.Id == id);
                    DataContext = trainee;
                    idTextBox.IsEnabled = false;
                    update = true;
                    AddressTextBox.Address =( trainee.Address != null) ? trainee.Address : null;
                }
                catch
                {
                    Close();
                }
            }
            BoxColumnGear.ItemsSource= Enum.GetValues(typeof(Gear));

            AddressTextBox.TextChanged += AddressTextBox_TextChanged;
            //set combox source
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(Gear));

            //set the choose license 
            ChooseLicense.ItemsSource = Enum.GetValues(typeof(LicenseType));
            ChooseLicense.SelectedItem = LicenseType.A;

            //initialze the license grid
            if (id != 0)
                foreach (var item in trainee.LicenseTypeLearning)
                    licenses.Add(item);
            LicenseDataGrid.ItemsSource = licenses;

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
                //update the licnse
                trainee.LicenseTypeLearning = licenses.ToList();

                //update or add the trainee
                if (update)
                    _blimp.UpdateTrainee(trainee);
                else
                    _blimp.AddTrainee(trainee);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        ///     On address text box changed update the address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trainee.Address = AddressTextBox.Address;
            }
            catch
            {
                AddressTextBox.Address = null;
            }
        }

        /// <summary>
        ///     On add license click, add a new licnese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLicnseButton_Click(object sender, RoutedEventArgs e)
        {
            //if he is learning it already
            if (licenses.Any(x => x.License == (LicenseType) ChooseLicense.SelectedItem))
                return;

            //Add the new license
            var number = int.Parse(NumberOfLessonsTextBox.Text);
            licenses.Add(new LessonsAndType
            {
                License = (LicenseType) ChooseLicense.SelectedItem,
                NumberOfLessons = number,
                ReadyForTest = number > Configuration.MinLessons,
                GearType = Gear.Automatic
            });
        }

        /// <summary>
        ///     On remove click, remove the license
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLicnseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                licenses.Remove(licenses.First(x => x.License == (LicenseType) ChooseLicense.SelectedItem));
            }
            catch
            {
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
            }
        }

        /// <summary>
        ///     When an error occured  in the data binding
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
    }
}