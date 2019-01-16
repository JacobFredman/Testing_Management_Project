using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BE;
using MahApps.Metro.Controls;
using PLWPF.Nofitications;

namespace PLWPF.Admin
{
    /// <summary>
    ///     Logic for setting window
    /// </summary>
    public partial class Settings : MetroWindow
    {
        /// <summary>
        /// collection  for the criteria
        /// </summary>
        private readonly ObservableCollection<string> _criteria = new ObservableCollection<string>();

        /// <summary>
        /// C-tor for settings
        /// </summary>
        public Settings()
        {
            InitializeComponent();

            //copy the setting from Configuration to the window
            MinLessonsBox.Value = Configuration.MinLessons;
            MinTesterAgeBox.Value = Configuration.MinTesterAge;
            MinTimeBetweenTestsBox.Value = Configuration.MinTimeBetweenTests;
            MinTraineeAgeBox.Value = Configuration.MinTraineeAge;
            MinimumCriterionsBox.Value = Configuration.MinimumCriteria;
            PercentOfCritirionsToPassTestBox.Value = Configuration.PercentOfCriteriaToPassTest;
            //copy criteria to the collection
            foreach (var item in Configuration.Criteria)
                _criteria.Add(item);
            CriterionsListBox.DataContext = _criteria;

            //Set theme ComBox
            Color.ItemsSource = new List<string> { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
            Color.SelectedItem = Configuration.Color;

            if (Configuration.Theme == "Light")
                LightTheme.IsChecked = true;
            else
                DarkTheme.IsChecked = true;
        }

        /// <summary>
        /// On Add new criterion click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NewCriterionBox.Text != "")
                    _criteria.Add(NewCriterionBox.Text);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// on remove selected criterion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _criteria.Remove(_criteria.First(x => x == CriterionsListBox.SelectedItem.ToString()));
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// on Save click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if the password are different
                if (PasswordBox.Password != RePasswordBox.Password)
                {
                    ExceptionMessage.Show("Password Are not Matching");
                    PasswordBox.Password = "";
                    RePasswordBox.Password = "";
                    return;
                }

                //if there is a password but not a user name
                if (PasswordBox.Password != "" && UserNameBox.Text == "")
                {
                    ExceptionMessage.Show("Username Can't Be Empty");
                    PasswordBox.Password = "";
                    RePasswordBox.Password = "";
                    return;
                }

                if (LightTheme.IsChecked == true)
                {
                    App.SetTheme(Color.SelectedItem.ToString(), "Light", Configuration.Color,
                        Configuration.Theme, false);
                    Configuration.Theme = "Light";
                }
                else
                {
                    App.SetTheme(Color.SelectedItem.ToString(), "Dark", Configuration.Color,
                        Configuration.Theme, false);
                    Configuration.Theme = "Dark";
                }

                Configuration.Color = Color.SelectedItem.ToString();


                //if there is a user name and password
                if (UserNameBox.Text != "")
                {
                    Configuration.AdminUser = UserNameBox.Text;
                    Configuration.AdminPassword = PasswordBox.Password;
                }

                //copy Back the criteria
                Configuration.Criteria = new string[_criteria.Count];
                var i = 0;
                foreach (var item in _criteria)
                {
                    Configuration.Criteria[i] = item;
                    i++;
                }

                //update the configurations
                Configuration.MinLessons = (uint) MinLessonsBox.Value;
                Configuration.MinTesterAge = (uint) MinTesterAgeBox.Value;
                Configuration.MinTimeBetweenTests = (uint) MinTimeBetweenTestsBox.Value;
                Configuration.MinTraineeAge = (uint) MinTraineeAgeBox.Value;
                Configuration.MinimumCriteria = (uint) MinimumCriterionsBox.Value;
                Configuration.PercentOfCriteriaToPassTest = (uint) PercentOfCritirionsToPassTestBox.Value;

                BL.FactoryBl.GetObject.SaveSettings();
                Close();
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message, ex.ToString());
            }
        }        
    }
}