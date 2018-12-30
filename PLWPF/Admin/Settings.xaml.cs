using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BE;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace PLWPF.Admin
{
    /// <summary>
    ///Logic for setting window
    /// </summary>
    public partial class Settings : MetroWindow
    {
        //collection  for the criterions
        private ObservableCollection<string> criterions = new ObservableCollection<string>();
        public Settings()
        {
            InitializeComponent();

            //copy the setting from Configuration to the window
            this.MinLessonsBox.Value = Configuration.MinLessons;
            this.MinTesterAgeBox.Value = Configuration.MinTesterAge;
            this.MinTimeBetweenTestsBox.Value = Configuration.MinTimeBetweenTests;
            this.MinTraineeAgeBox.Value = Configuration.MinTraineeAge;
            this.MinimumCriterionsBox.Value = Configuration.MinimumCriterions;
            this.PercentOfCritirionsToPassTestBox.Value = Configuration.PercentOfCritirionsToPassTest;
            //copy criterions to the collection
            foreach (var item in Configuration.Criterions)
                criterions.Add(item);
            CriterionsListBox.DataContext = criterions;

            //Set theme ComBox
            Theme.ItemsSource = new List<string>() {"Orange", "Green", "Gray", "Light Blue" ,"Blue"};
            Theme.SelectedItem = "Light Blue";

        }

        //On Add new criterion click
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(NewCriterionBox.Text!="")
                    criterions.Add(NewCriterionBox.Text);
            }
            catch { }
        }

        //on remove selected criterion
        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                criterions.Remove(criterions.First(x => x == CriterionsListBox.SelectedItem.ToString()));
            }
            catch { }
        }

        //on Save click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //if the password are different
            if (PasswordBox.Password != RePasswordBox.Password  )
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

            //copy Back the criterions
            Configuration.Criterions=new string[criterions.Count];
            int i = 0;
            foreach (var item in criterions)
            {
                Configuration.Criterions[i] = item;
                i++;
            }

            //update the configurations
            Configuration.MinLessons = (uint)this.MinLessonsBox.Value;
            Configuration.MinTesterAge = (uint)this.MinTesterAgeBox.Value;
            Configuration.MinTimeBetweenTests = (uint)this.MinTimeBetweenTestsBox.Value;
            Configuration.MinTraineeAge = (uint)this.MinTraineeAgeBox.Value;
            Configuration.MinimumCriterions = (uint)this.MinimumCriterionsBox.Value;
            Configuration.PercentOfCritirionsToPassTest = (uint)this.PercentOfCritirionsToPassTestBox.Value;
            Close();

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
