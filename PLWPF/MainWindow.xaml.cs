using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BE;
using BE.MainObjects;
using BE.Routes;
using BL;
using PLWPF.Admin;

namespace PLWPF
{
    /// <summary>
    ///     Login window
    /// </summary>
    public partial class MainWindow : Window
    {
        //get an object of BL
        private readonly IBL _blimp = FactoryBl.GetObject;

        public MainWindow()
        {
            InitializeComponent();

     

            //Add information to test the program
            AddInfo();

            //On first login enter username and password
            if (Configuration.firtOpenProgram)
            {
                AdminUsernameTextBox.Text = Configuration.AdminUser;
                AdminPasswordTextBox.Password = Configuration.AdminPassword;
            }
            Configuration.firtOpenProgram = false;
        }

   

        /// <summary>
        ///     for debugging only !!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        private void AddInfo()
        {
            try
            {
                var test = new LessonsAndType {License = LicenseType.A, NumberOfLessons = 30, ReadyForTest = true};
                var list = new List<LessonsAndType>();
                list.Add(test);
                _blimp.AddTrainee(new Trainee(319185997, Gender.Male, "Elisha", "Mayer")
                {
                    BirthDate = new DateTime(1995, 1, 1), LicenseTypeLearning = list,
                    Address = new Address("Jerusalem"),
                    EmailAddress = "elisja.sc@gmail.com"
                });

                _blimp.AddTrainee(new Trainee(037982519, Gender.Male, "Jacob", "Fredman")
                {
                    BirthDate = new DateTime(1985, 12, 29),
                    LicenseTypeLearning = list,
                    Address = new Address("Jerusalem"),
                    EmailAddress = "jacov141@gmail.com"
                });
               
                _blimp.AddTrainee(new Trainee(319185971, Gender.Male, "Moshe", "Levi")
                    {BirthDate = new DateTime(1987, 1, 1)});
                _blimp.AddTrainee(new Trainee(314661133, Gender.Male, "Bob", "Ray")
                    {BirthDate = new DateTime(1980, 1, 1)});
                _blimp.AddTrainee(new Trainee(324126747, Gender.Male, "Avi", "Alon")
                    {BirthDate = new DateTime(1970, 1, 1)});
                _blimp.AddTrainee(new Trainee(326591088, Gender.Female, "Avia", "Abu")
                    {BirthDate = new DateTime(1999, 1, 1)});
                _blimp.AddTrainee(new Trainee(342533643, Gender.Male, "Gil", "Rami")
                    {BirthDate = new DateTime(2000, 1, 1)});
                _blimp.AddTrainee(new Trainee(339794166, Gender.Male, "David", "Aboulafia")
                    {BirthDate = new DateTime(2000, 1, 1)});
                _blimp.AddTrainee(new Trainee(336390885, Gender.Male, "Shlomo", "Simchon")
                    {BirthDate = new DateTime(1991, 1, 1)});
                _blimp.AddTrainee(new Trainee(332484609, Gender.Female, "Gavriela", "Abuxsis")
                    {BirthDate = new DateTime(1991, 1, 1)});
                _blimp.AddTrainee(new Trainee(332307065, Gender.Female, "Yafa", "Alaluf")
                    {BirthDate = new DateTime(1999, 1, 1)});
                _blimp.AddTrainee(new Trainee(332270446, Gender.Male, "Dudu", "Tapiro")
                    {BirthDate = new DateTime(2000, 1, 1)});
                _blimp.AddTrainee(new Trainee(329043459, Gender.Male, "Pinchas", "Moshe")
                    {BirthDate = new DateTime(1988, 1, 1)});

                var sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                var list2 = new List<LicenseType>();
                list2.Add(LicenseType.A);
                _blimp.AddTester(new Tester(328729660, "meir", "")
                {
                    BirthDate = new DateTime(1945, 1, 1), Address = new Address("בית שמש"), MaxDistance = 10000000,
                    Schedule = sch, MaxWeekExams = 10, LicenseTypeTeaching = list2
                });
                sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                _blimp.AddTester(new Tester(324040443, "Maoz,", "Shectman")
                    {BirthDate = new DateTime(1970, 1, 1), Address = new Address("Tel Aviv"), Schedule = sch});
                sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                _blimp.AddTester(new Tester(323873182, "Eliran", "Franko")
                    {BirthDate = new DateTime(1961, 1, 1), Address = new Address("Beit shemesh"), Schedule = sch});
                sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                _blimp.AddTester(new Tester(323082321, "David", "Arbiv")
                    {BirthDate = new DateTime(1950, 1, 1), Address = new Address("Holon"), Schedule = sch});

                var test1 = new Test(328729660, 319185997)
                {
                    TestTime = new DateTime(2018, 12, 18, 12, 00, 00),
                   AddressOfBeginningTest = new Address("jerusalem"),
                    LicenseType = LicenseType.A
                };

                var test2 = new Test(328729660, 037982519)
                {
                    TestTime = new DateTime(2018, 12, 19, 12, 00, 00),
                    AddressOfBeginningTest = new Address("jerusalem"),
                    LicenseType = LicenseType.A
                };
                try
                {
                  //  test1.SetRouteAndAddressToTest(new Address("jerusalem"));
                }
                catch
                {

                }

                test2.Passed = true;
               // _blimp.AddTest(test1);
                _blimp.AddTest(test2);
                _blimp.SendEmailToAllTraineeAfterTest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// check the id .if it is correct the enable the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterIDTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tools.CheckID_IL((uint.Parse(TesterIDTestBox.Text))))
                {
                    TesterLoginButton.IsEnabled = true;
                    TesterIDTestBox.BorderBrush = Brushes.Black;
                }
                else throw new Exception();
            }
            catch
            {
                TesterLoginButton.IsEnabled = false;
                TesterIDTestBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// check the id .if it is correct the enable the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeIDTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tools.CheckID_IL((uint.Parse(TraineeIDTestBox.Text))))
                {
                    TraineeLoginButton.IsEnabled = true;
                    TraineeIDTestBox.BorderBrush = Brushes.Black;
                }
                else throw new Exception();
            }
            catch
            {
                TraineeLoginButton.IsEnabled = false;
                TraineeIDTestBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// On login click open trainee window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hide();
                var win = new TraineeWin(int.Parse(TraineeIDTestBox.Text));
                win.ShowDialog();
                Show();
            }
            catch
            {
                MessageBox.Show("Trainee doesn't exist please contact the administrator.");
            }
        }

        /// <summary>
        /// On login click open tester window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hide();
                var win = new TesterWin(int.Parse(TesterIDTestBox.Text));
                win.ShowDialog();
                Show();
            }
            catch
            {
                MessageBox.Show("Tester doesn't exist please contact the administrator.");
            }
        }

        /// <summary>
        /// On login Click open administrator window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminUsernameTextBox.Text == Configuration.AdminUser &&
                AdminPasswordTextBox.Password == Configuration.AdminPassword)
            {
                AdminUsernameTextBox.Text = "";
                AdminPasswordTextBox.Password = "";
                Hide();
                var win =new Administrator();
                win.ShowDialog();
                Show();
            }
            else
            {
                MessageBox.Show("Wrong Password or UserName.");
            }
        }
    }
}