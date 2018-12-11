using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BE.MainObjects;
using BE;
using BL;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections;



namespace PLWPF.Admin.ManageTester
{
    /// <summary>
    /// Interaction logic for AddTester.xaml
    /// </summary>
    public partial class AddTester : Window
    {
        private Tester tester = new Tester();
        private IBL _blimp = FactoryBl.GetObject;
        private bool update = false;
        private ObservableCollection<BE.MainObjects.LessonsAndType> licenses = new ObservableCollection<LessonsAndType>();
        private List<string> errorMessage = new List<string>();
        public AddTester(uint id = 0)
        {
            InitializeComponent();
            if (id == 0)
                DataContext = tester;
            else
            {
                try
                {
                    tester = _blimp.AllTesters.First(x => x.Id == id);
                    DataContext = tester;
                    idTextBox.IsEnabled = false;
                    update = true;
                    AllWeek.IsEnabled = false;
                    AllWeek.IsChecked = false;
                    DayWeek.IsEnabled = true;
                    AddressTextBox.Text = (tester.Address!=null)? tester.Address.ToString():"";
                }
                catch
                {
                    this.Close();
                }
            }
            if (!update)
            {
                tester.BirthDate = DateTime.Now.Date;
                tester.Schedule = new WeekSchedule();
            }
            genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            Chooselicense.ItemsSource= Enum.GetValues(typeof(BE.LicenseType));
            var list = new List<DayOfWeek>();
            foreach (var item in (Enum.GetValues(typeof(DayOfWeek))))
                list.Add((DayOfWeek)item);
            DayWeek.ItemsSource = list.Take(5);
            var hours = new List<string>();
            for(int i = 0; i < 24; i++)
            {
                hours.Add(string.Format("{0:00}:00",i));
            }
            ChooseHours.ItemsSource = hours;
            DayWeek.SelectedItem = DayOfWeek.Sunday;
            DayWeek.SelectedItem = "All Days";
            if (tester.LicenseTypeTeaching == null) tester.LicenseTypeTeaching = new List<LicenseType>();
            if (update)
            {
                foreach (var item in tester.LicenseTypeTeaching)
                    Chooselicense.SelectedItems.Add(item);

            }
         

            Save.IsEnabled = false;
        }

        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (BE.Tools.CheckID_IL(uint.Parse(idTextBox.Text)))
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

        private void EmailAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tester.Address = new BE.Routes.Address(AddressTextBox.Text);
            }
            catch
            {
                AddressTextBox.Text = "";
            }
        }
        private void validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) errorMessage.Add(e.Error.Exception.Message);
            else errorMessage.Remove(e.Error.Exception.Message);
            ErrorMessage.Text = "";
            foreach (var item in errorMessage)
            {
                ErrorMessage.Text += item + "\n";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                tester.Address = new BE.Routes.Address(AddressTextBox.Text);
                if (update)
                    _blimp.UpdateTester(tester);
                else
                    _blimp.AddTester(tester);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Chooselicense_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tester.LicenseTypeTeaching = new List<LicenseType>();
            foreach(var item in Chooselicense.SelectedItems)
            {
                tester.LicenseTypeTeaching.Add((LicenseType)item);
            }
        }

        private void AllWeek_Checked(object sender, RoutedEventArgs e)
        {
            DayWeek.IsEnabled = false;
        }

        private void AllWeek_Unchecked(object sender, RoutedEventArgs e)
        {
            DayWeek.IsEnabled = true;
        }

        private void ChooseHours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllWeek.IsChecked == true)
            {
                foreach (var day in tester.Schedule.days)
                {
                    day.ClearHours();
                    foreach (var hour in ChooseHours.SelectedItems)
                        day.Hours[int.Parse(((string)hour).Substring(0,2))] = true;
                }
            }
            else
            {
                var day = tester.Schedule[(DayOfWeek)DayWeek.SelectedItem];
                foreach (var hour in ChooseHours.SelectedItems)
                    day.Hours[int.Parse(((string)hour).Substring(0, 2))] = true;
            }
        }

        private void DayWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var day = tester.Schedule[(DayOfWeek)DayWeek.SelectedItem];
            int i = 0;
            var list = new List<string>();
            foreach (var hour in day.Hours)
            {
                if (hour) list.Add(string.Format("{0:00}:00", i));
                i++;
            }
            ChooseHours.UnselectAll();
            foreach (var item in list)
            {
                ChooseHours.SelectedItems.Add(item);
            }
        }
    }
}
