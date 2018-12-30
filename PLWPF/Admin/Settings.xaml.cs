using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        //collection  for the criteria
        private readonly ObservableCollection<string> _criteria = new ObservableCollection<string>();

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
            Theme.ItemsSource = new List<string> {"Orange", "Green", "Gray", "Light Blue", "Blue"};
            Theme.SelectedItem = "Light Blue";
        }

        //On Add new criterion click
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

        //on remove selected criterion
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

        //on Save click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if the password are different
                if (PasswordBox.Password != RePasswordBox.Password)
                {
                    MessageBox.Show("Password Are not Matching");
                    PasswordBox.Password = "";
                    RePasswordBox.Password = "";
                    return;
                }

                //if there is a password but not a user name
                if (PasswordBox.Password != "" && UserNameBox.Text == "")
                {
                    MessageBox.Show("Username Can't Be Empty");
                    PasswordBox.Password = "";
                    RePasswordBox.Password = "";
                    return;
                }

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
                Close();
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message,ex.ToString());
            }
        }


        //Change theme
        private void Theme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Theme.SelectedItem.ToString())
            {
                case "Orange":
                    Application.Current.Resources["Background"] = Brushes.SandyBrown;
                    break;
                case "Green":
                    Application.Current.Resources["Background"] = Brushes.LightGreen;
                    break;
                case "Gray":
                    Application.Current.Resources["Background"] = Brushes.LightGray;
                    break;
                case "Light Blue":
                    Application.Current.Resources["Background"] = Brushes.Aquamarine;
                    break;
                case "Blue":
                    Application.Current.Resources["Background"] = Application.Current.Resources["AccentBaseColorBrush"];
                    break;
            }
        }
    }
}