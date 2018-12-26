using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BE;

namespace PLWPF.Admin
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private ObservableCollection<string> criterions = new ObservableCollection<string>();
        public Settings()
        {
            InitializeComponent();
            this.MinLessonsBox.Number = Configuration.MinLessons;
            this.MinTesterAgeBox.Number = Configuration.MinTesterAge;
            this.MinTimeBetweenTestsBox.Number = Configuration.MinTimeBetweenTests;
            this.MinTraineeAgeBox.Number = Configuration.MinTraineeAge;
            this.MinimumCriterionsBox.Number = Configuration.MinimumCriterions;
            this.PercentOfCritirionsToPassTestBox.Number = Configuration.PercentOfCritirionsToPassTest;
            foreach (var item in Configuration.Criterions)
                criterions.Add(item);
            CriterionsListBox.DataContext = criterions;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(NewCriterionBox.Text!="")
                    criterions.Add(NewCriterionBox.Text);
            }
            catch { }
        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                criterions.Remove(criterions.First(x => x == CriterionsListBox.SelectedItem.ToString()));
            }
            catch { }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password != RePasswordBox.Password  )
            {
                MessageBox.Show("Password Are not Matching");
                PasswordBox.Password = "";
                RePasswordBox.Password = "";
                return;
            }

            if (PasswordBox.Password != "" && UserNameBox.Text == "")
            {
                MessageBox.Show("Username Can't Be Empty");
                PasswordBox.Password = "";
                RePasswordBox.Password = "";
                return;
            }

            if (UserNameBox.Text != "")
            {
                Configuration.AdminUser = UserNameBox.Text;
                Configuration.AdminPassword = PasswordBox.Password;
            }

            Configuration.Criterions=new string[criterions.Count];
            int i = 0;
            foreach (var item in criterions)
            {
                Configuration.Criterions[i] = item;
                i++;
            }


            Configuration.MinLessons = this.MinLessonsBox.Number;
            Configuration.MinTesterAge = this.MinTesterAgeBox.Number;
            Configuration.MinTimeBetweenTests = this.MinTimeBetweenTestsBox.Number;
            Configuration.MinTraineeAge = this.MinTraineeAgeBox.Number;
            Configuration.MinimumCriterions = this.MinimumCriterionsBox.Number;
            Configuration.PercentOfCritirionsToPassTest = this.PercentOfCritirionsToPassTestBox.Number;
            Close();

        }
    }
}
