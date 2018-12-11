using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.Routes;
using BE.MainObjects;
using BL;
using System.Collections.Generic;

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
                    Admin.Administrator Win = new Admin.Administrator();
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
                var test = new LessonsAndType() { License = LicenseType.A, NumberOfLessons = 30, ReadyForTest = true };
                var list = new List<LessonsAndType>();
                list.Add(test);
                _blimp.AddTrainee(new Trainee(319185997, Gender.Male, "aaaaa", "bbbbb") { BirthDate = new DateTime(1991, 1, 1)
                    , LicenseTypeLearning =list,
                    Address = new Address("Jerusalem")
                });
                _blimp.AddTrainee(new Trainee(319185989, Gender.Male, "ccccc", "dddddd") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(319185971, Gender.Male, "sfsfss", "sdvzv") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(314661133, Gender.Male, "zzc", "zvzv") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(324126747, Gender.Male, "qrqf", "ghmgm") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(326591088, Gender.Male, "sdss", "cvb") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(342533643, Gender.Male, "zvvxv", "zvzv") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(339794166, Gender.Male, "xcvvxv", "zxvzv") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(336390885, Gender.Male, "zxvzv", "zxzvz") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(332484609, Gender.Male, "zxvzv", "zxvzvz") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(332307065, Gender.Male, "zzv", "Mazxvzvyer") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(332270446, Gender.Male, "zxvzv", "zxvzvzv") { BirthDate = new DateTime(1991, 1, 1) });
                _blimp.AddTrainee(new Trainee(329043459, Gender.Male, "ghg,g", "asfasfaf") { BirthDate = new DateTime(1991, 1, 1) });

                var sch = new WeekSchedule();
                    sch.AddHoursAllDays(0, 23);
                var list2 = new List<LicenseType>();
                    list2.Add(LicenseType.A);
                _blimp.AddTester(new Tester(328729660, "zvzv", "dsadada") { BirthDate = new DateTime(1960, 1, 1), Address = new Address("Hardera") ,Schedule= sch ,LicenseTypeTeaching=list2});
                _blimp.AddTester(new Tester(324040443, "vmm,", "vzxvz") { BirthDate = new DateTime(1960, 1, 1), Address = new Address("Tel Aviv"), Schedule = sch });
                _blimp.AddTester(new Tester(323873182, "zxvzvz", "zxzv") { BirthDate = new DateTime(1960, 1, 1), Address = new Address("Beit shemesh"), Schedule = sch });
                _blimp.AddTester(new Tester(323082321, "zxvzvz", "jljkl") { BirthDate = new DateTime(1960, 1, 1), Address = new Address("Holon"), Schedule = sch });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
