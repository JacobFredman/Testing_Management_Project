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
            Color.ItemsSource = new List<string> { "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna" };
            Color.SelectedItem = Configuration.Color;

            Theme.ItemsSource = new List<string> {"Light","Dark" };
            Theme.SelectedItem = Configuration.Theme;
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


                App.SetTheme(Color.SelectedItem.ToString(),Theme.SelectedItem.ToString(), Configuration.Color,Configuration.Theme,false);
                Configuration.Color = Color.SelectedItem.ToString();
                Configuration.Theme = Theme.SelectedItem.ToString();

                if (Configuration.Theme == "Light")
                {
                    switch (Configuration.Color)
                    {
                        case "Red":
                            Application.Current.Resources["Background"] = Brushes.LightCoral;
                            break;
                        case "Green":
                            Application.Current.Resources["Background"] = Brushes.LightGreen;
                            break;
                        case "Blue":
                            Application.Current.Resources["Background"] = Brushes.LightSkyBlue;
                            break;
                        case "Purple":
                            Application.Current.Resources["Background"] = Brushes.MediumPurple;
                            break;
                        case "Orange":
                            Application.Current.Resources["Background"] = Brushes.Orange;
                            break;
                        case "Lime":
                            Application.Current.Resources["Background"] = Brushes.LightGreen;
                            break;
                        case "Emerald":
                            Application.Current.Resources["Background"] = Brushes.LightGreen;
                            break;
                        case "Teal":
                            Application.Current.Resources["Background"] = Brushes.PaleTurquoise;
                            break;
                        case "Cyan":
                            Application.Current.Resources["Background"] = Brushes.LightCyan;
                            break;
                        case "Cobalt":
                            Application.Current.Resources["Background"] =Brushes.CornflowerBlue;
                            break;
                        case "Indigo":
                            Application.Current.Resources["Background"] = Brushes.IndianRed;
                            break;
                        case "Violet":
                            Application.Current.Resources["Background"] = Brushes.PaleVioletRed;
                            break;
                        case "Pink":
                            Application.Current.Resources["Background"] = Brushes.Pink;
                            break;
                        case "Magenta":
                            Application.Current.Resources["Background"] = Brushes.MediumOrchid;
                            break;
                        case "Crimson":
                            Application.Current.Resources["Background"] = Brushes.LightCoral;
                            break;
                        case "Sienna":
                            Application.Current.Resources["Background"] = Brushes.SandyBrown;
                            break;
                        case "Taupe":
                            Application.Current.Resources["Background"] = Brushes.Tan;
                            break;
                        case "Mauve":
                            Application.Current.Resources["Background"] = Brushes.AntiqueWhite;
                            break;
                        case "Steel":
                            Application.Current.Resources["Background"] = Brushes.LightSteelBlue;
                            break;
                        case "Olive":
                            Application.Current.Resources["Background"] = Brushes.LightGreen;
                            break;
                        case "Brown":
                            Application.Current.Resources["Background"] = Brushes.SandyBrown;
                            break;
                        case "Yellow":
                            Application.Current.Resources["Background"] = Brushes.LightGoldenrodYellow;
                            break;
                        case "Amber":
                            Application.Current.Resources["Background"] =Brushes.LightGoldenrodYellow;
                            break;


                    }

                }
                else
                {

                    Application.Current.Resources["Background"] = Brushes.Black;
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

                BL.FactoryBl.GetObject.SaveSettings();
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
            //switch (Theme.SelectedItem.ToString())
            //{
            //    case "Orange":
            //        Application.Current.Resources["Background"] = Brushes.SandyBrown;
            //        break;
            //    case "Green":
            //        Application.Current.Resources["Background"] = Brushes.LightGreen;
            //        break;
            //    case "Gray":
            //        Application.Current.Resources["Background"] = Brushes.LightGray;
            //        break;
            //    case "Light Blue":
            //        Application.Current.Resources["Background"] = Brushes.Aquamarine;
            //        break;
            //    case "Blue":
            //        Application.Current.Resources["Background"] = Application.Current.Resources["AccentBaseColorBrush"];
            //        break;
            //}
        }
    }
}