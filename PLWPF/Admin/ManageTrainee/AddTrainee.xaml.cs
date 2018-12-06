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
using BL;

namespace PLWPF.Admin.ManageTrainee
{
    /// <summary>
    /// Interaction logic for AddTrainee.xaml
    /// </summary>
    public partial class AddTrainee : Window
    {
        private Trainee trainee = new Trainee();
        private IBL _blimp = FactoryBl.GetObject;
        public AddTrainee()
        {
            InitializeComponent();
            DataContext = trainee;
            genderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            gearTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gear));
            trainee.BirthDate= DateTime.Now.Date;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

  

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try {
                _blimp.AddTrainee(trainee);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
