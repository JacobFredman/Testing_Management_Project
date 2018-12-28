using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
            this.MinLessonsBox.Number = Configuration.MinLessons;
            this.MinTesterAgeBox.Number = Configuration.MinTesterAge;
            this.MinTimeBetweenTestsBox.Number = Configuration.MinTimeBetweenTests;
            this.MinTraineeAgeBox.Number = Configuration.MinTraineeAge;
            this.MinimumCriterionsBox.Number = Configuration.MinimumCriterions;
            this.PercentOfCritirionsToPassTestBox.Number = Configuration.PercentOfCritirionsToPassTest;
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
            Configuration.MinLessons = this.MinLessonsBox.Number;
            Configuration.MinTesterAge = this.MinTesterAgeBox.Number;
            Configuration.MinTimeBetweenTests = this.MinTimeBetweenTestsBox.Number;
            Configuration.MinTraineeAge = this.MinTraineeAgeBox.Number;
            Configuration.MinimumCriterions = this.MinimumCriterionsBox.Number;
            Configuration.PercentOfCritirionsToPassTest = this.PercentOfCritirionsToPassTestBox.Number;
            Close();

        }

        private void Theme_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Resources["Background"] = Brushes.LightSalmon;
        }

        //Change theme
        private void Theme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Theme.SelectedItem.ToString())
            {
                case "Orange":
                    Application.Current.Resources["Background"] = Brushes.Orange;
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
