using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
using BL;

namespace PLWPF.Admin.ManageTrainee
{
    /// <summary>
    /// Add or update trainee
    /// </summary>
    public partial class AddTrainee : Window
    {
        private Trainee trainee = new Trainee();
        private IBL _blimp = FactoryBl.GetObject;
        private bool update=false;
        private ObservableCollection<BE.MainObjects.LessonsAndType> licenses = new ObservableCollection<LessonsAndType>();
        private List<string> errorMessage = new List<string>();
        public AddTrainee(uint id=0)
        {
            InitializeComponent();
            if(id==0)
                 DataContext = trainee;
            else
            {
                try
                {
                    trainee = _blimp.AllTrainee.First(x => x.Id == id);
                    DataContext = trainee;
                    idTextBox.IsEnabled = false;
                    update = true;
                    AddressTextBox.Text =(trainee.Address!=null)? trainee.Address.ToString():"";
                }
                catch
                {
                    this.Close();
                }

            }
            genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gear));
   
            ChooseLicense.ItemsSource= Enum.GetValues(typeof(BE.LicenseType));
            ChooseLicense.SelectedItem = BE.LicenseType.A;
            if (id != 0)
            {
                foreach(var item in trainee.LicenseTypeLearning)
                {
                    licenses.Add(item);
                }

            }
            LicenseDataGrid.ItemsSource = licenses;
            
            if (!update)trainee.BirthDate= DateTime.Now.Date;
            Save.IsEnabled = false;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

  

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            try {
                trainee.LicenseTypeLearning = licenses.ToList();
                if (update)
                    _blimp.UpdateTrainee(trainee);
                else
                    _blimp.AddTrainee(trainee);
                this.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                trainee.Address = new BE.Routes.Address(AddressTextBox.Text);
            }
            catch
            {
                AddressTextBox.Text = "";
            }
        }

        private void AddLicnseButton_Click(object sender, RoutedEventArgs e)
        {

            if (licenses.Any(x => x.License == (BE.LicenseType)ChooseLicense.SelectedItem))
                return;
            var number = int.Parse(NumberOfLessonsTextBox.Text);
            licenses.Add(new BE.MainObjects.LessonsAndType
            {
                License = (BE.LicenseType)ChooseLicense.SelectedItem,
                NumberOfLessons = number,
                ReadyForTest = number > BE.Configuration.MinLessons
            });
           // LicenseDataGrid.ItemsSource = trainee.LicenseTypeLearning;
        }

        private void RemoveLicnseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                licenses.Remove(licenses.First(x => x.License == (BE.LicenseType)ChooseLicense.SelectedItem));
            }
            catch { }
        }

     

        private void RefreshLicense(object sender, EventArgs e)
        {
            try
            {
                LicenseDataGrid.Items.Refresh();
            }
            catch { }
        }

        private void validation_Error(object sender, ValidationErrorEventArgs e) { 
            if (e.Action == ValidationErrorEventAction.Added) errorMessage.Add(e.Error.Exception.Message); 
            else errorMessage.Remove(e.Error.Exception.Message);
            ErrorMessage.Text = "";
            foreach (var item in errorMessage)
            {
                ErrorMessage.Text += item + "\n";
            }
        }
    }
}
