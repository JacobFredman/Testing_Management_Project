using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.Routes;
using BE.MainObjects;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBL _blimp = FactoryBl.GetObject;
        public MainWindow()
        {
            InitializeComponent();

            AddInfo();
        }

        /// <summary>
        /// Error in fild in invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (IdTextBox.Text != Configuration.AdminUser && !Tools.CheckID_IL(uint.Parse(IdTextBox.Text)))
                    IdErrorLabel.Visibility = Visibility.Visible;
                else
                    IdErrorLabel.Visibility = Visibility.Hidden;
            }
            catch
            {
                IdErrorLabel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Error in fild in invalid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BirthDateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var date = DateTime.MaxValue;
               
                if (BirthDateTextBox.Text != Configuration.AdminPassword && !DateTime.TryParse(BirthDateTextBox.Text, out date))
                    BirthDateError.Visibility = Visibility.Visible;
                else
                    BirthDateError.Visibility = Visibility.Hidden;
            }
            catch
            {
                BirthDateError.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Try to login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if it is the admin
                if (IdTextBox.Text == Configuration.AdminUser && BirthDateTextBox.Text == Configuration.AdminPassword)
                {
                    Admin Win = new Admin();
                    Hide();
                    Win.ShowDialog();
                    IdTextBox.Text = "";
                    BirthDateTextBox.Text = "";
                    IdErrorLabel.Visibility = Visibility.Hidden;
                    BirthDateError.Visibility = Visibility.Hidden;
                    Show();
                    return;
                }
                    

                //if it is a tester or a trainee
                switch (_blimp.GetTypeFromId(int.Parse(IdTextBox.Text), DateTime.Parse(BirthDateTextBox.Text)))
                {

                    //it is a testr
                    case "BE.MainObjects.Tester":   
                        TesterWin Win = new TesterWin(int.Parse(IdTextBox.Text));
                        Hide();
                        Win.ShowDialog();
                        IdTextBox.Text = "";
                        BirthDateTextBox.Text = "";
                        IdErrorLabel.Visibility = Visibility.Hidden;
                        BirthDateError.Visibility = Visibility.Hidden;
                        Show();
                        return;

                    //it is a trainee
                    case "BE.MainObjects.Trainee":
                        TraineeWin Win2 = new TraineeWin(int.Parse(IdTextBox.Text));
                        Hide();
                        Win2.ShowDialog();
                        IdTextBox.Text = "";
                        BirthDateTextBox.Text = "";
                        IdErrorLabel.Visibility = Visibility.Hidden;
                        BirthDateError.Visibility = Visibility.Hidden;
                        Show();
                        return;

                    default:
                        break;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// for debbuging only !!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        private void AddInfo()
        {
            try
            {
                _blimp.AddTrainee(new Trainee(319185997, Gender.Male, "Elisha", "Mayer") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTester(new Tester(319185989, "Amnon", "Mayer") { BirthDate = new DateTime(1960, 1, 1), Address = new Address("jerusalem") });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
