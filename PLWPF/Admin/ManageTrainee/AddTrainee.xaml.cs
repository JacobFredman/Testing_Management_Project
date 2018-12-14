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
        //true if it is an update
        private bool update=false;
        //collection to work with the license
        private ObservableCollection<BE.MainObjects.LessonsAndType> licenses = new ObservableCollection<LessonsAndType>();
        //all the exceptions
        private List<string> errorMessage = new List<string>();

        /// <summary>
        /// Add or update an trainee
        /// </summary>
        /// <param name="id">if it is an update then put the trainee id </param>
        public AddTrainee(uint id=0)
        {
            InitializeComponent();

            //initialize as add
            if (id == 0)
            {
                DataContext = trainee;
                trainee.BirthDate = DateTime.Now.Date;
            }
            //initialize as update
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
            //set combox source
            genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gear));
   
            //set the choose license 
            ChooseLicense.ItemsSource= Enum.GetValues(typeof(BE.LicenseType));
            ChooseLicense.SelectedItem = BE.LicenseType.A;

            //initialze the license grid
            if (id != 0)
            {
                foreach(var item in trainee.LicenseTypeLearning)
                {
                    licenses.Add(item);
                }

            }
            LicenseDataGrid.ItemsSource = licenses;
            
            //disable the Save button
            Save.IsEnabled = false;
        }

        /// <summary>
        /// On click save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {

            try {
                //update the licnse
                trainee.LicenseTypeLearning = licenses.ToList();

                //update or add the trainee
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

        /// <summary>
        /// On Id text box change check the id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// On address text box changed update the address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// On add license click, add a new licnese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLicnseButton_Click(object sender, RoutedEventArgs e)
        {
            //if he is learning it already
            if (licenses.Any(x => x.License == (BE.LicenseType)ChooseLicense.SelectedItem))
                return;

            //Add the new license
            var number = int.Parse(NumberOfLessonsTextBox.Text);
            licenses.Add(new LessonsAndType
            {
                License = (BE.LicenseType)ChooseLicense.SelectedItem,
                NumberOfLessons = number,
                ReadyForTest = number > BE.Configuration.MinLessons
            });
        }

        /// <summary>
        /// On remove click, remove the license
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveLicnseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                licenses.Remove(licenses.First(x => x.License == (BE.LicenseType)ChooseLicense.SelectedItem));
            }
            catch { }
        }

        /// <summary>
        /// Refresh the license grid after update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshLicense(object sender, EventArgs e)
        {
            try
            {
                LicenseDataGrid.Items.Refresh();
            }
            catch { }
        }

        /// <summary>
        /// When an error occured  in the data binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
