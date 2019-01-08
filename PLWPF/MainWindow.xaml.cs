using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BE;
using BE.MainObjects;
using BE.Routes;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Admin;
using PLWPF.Nofitications;

namespace PLWPF
{
    /// <summary>
    ///     Login window
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //get an object of BL
        private readonly IBL _blimp = FactoryBl.GetObject;

        public MainWindow()
        {
            InitializeComponent();
            var bl = BL.FactoryBl.GetObject;

            //Add information to test the program
            AddInfo();

            //On first login enter username and password
            if (Configuration.FirstOpenProgram)
            {
                AdminUsernameTextBox.Text = Configuration.AdminUser;
                AdminPasswordTextBox.Password = Configuration.AdminPassword;
            }

            AdminUsernameTextBox.Focus();
            Configuration.FirstOpenProgram = false;
        }

      

        /// <summary>
        ///     for debugging only !!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        private void AddInfo()
        {
            try
            {
                //var test = new LessonsAndType { License = LicenseType.A, NumberOfLessons = 30, ReadyForTest = true };
                //var list = new List<LessonsAndType> { test };
                //_blimp.SetTest(new Trainee(319185997, Gender.Male, "Elisha", "Mayer")
                //{
                //    BirthDate = new DateTime(1995, 1, 1),
                //    LicenseTypeLearning = list,
                //    Address = new Address("הועד הלאומי 14, ירושלים, ישראל"),
                //    EmailAddress = "elisja.sc@gmail.com"
                //});
                //_blimp.SetTest(new Trainee(037982519, Gender.Male, "Jacob", "Fredman")
                //{ BirthDate = new DateTime(1985, 1, 12) });
                //_blimp.SetTest(new Trainee(319185971, Gender.Male, "Moshe", "Levi")
                //{ BirthDate = new DateTime(1987, 1, 1) });
                //_blimp.SetTest(new Trainee(314661133, Gender.Male, "Bob", "Ray")
                //{ BirthDate = new DateTime(1980, 1, 1) });
                //_blimp.SetTest(new Trainee(324126747, Gender.Male, "Avi", "Alon")
                //{ BirthDate = new DateTime(1970, 1, 1) });
                //_blimp.SetTest(new Trainee(326591088, Gender.Female, "Avia", "Abu")
                //{ BirthDate = new DateTime(1999, 1, 1) });
                //_blimp.SetTest(new Trainee(342533643, Gender.Male, "Gil", "Rami")
                //{ BirthDate = new DateTime(2000, 1, 1) });
                //_blimp.SetTest(new Trainee(339794166, Gender.Male, "David", "Aboulafia")
                //{ BirthDate = new DateTime(2000, 1, 1) });
                //_blimp.SetTest(new Trainee(336390885, Gender.Male, "Shlomo", "Simchon")
                //{ BirthDate = new DateTime(1991, 1, 1) });
                //_blimp.SetTest(new Trainee(332484609, Gender.Female, "Gavriela", "Abuxsis")
                //{ BirthDate = new DateTime(1991, 1, 1) });
                //_blimp.SetTest(new Trainee(332307065, Gender.Female, "Yafa", "Alaluf")
                //{ BirthDate = new DateTime(1999, 1, 1) });
                //_blimp.SetTest(new Trainee(332270446, Gender.Male, "Dudu", "Tapiro")
                //{ BirthDate = new DateTime(2000, 1, 1) });
                //_blimp.SetTest(new Trainee(329043459, Gender.Male, "Pinchas", "Moshe")
                //{ BirthDate = new DateTime(1988, 1, 1) });

                var sch = new WeekSchedule();
                sch.AddHoursAllDays(12, 15);
                var list2 = new List<LicenseType>();
                list2.Add(LicenseType.A);
                _blimp.AddTester(new Tester(328729660, "meir", "")
                {
                    BirthDate = new DateTime(1945, 1, 1), Address = new Address("הרצל 30, בית שמש, ישראל"),
                    MaxDistance = 1000,
                    Schedule = sch, MaxWeekExams = 10, LicenseTypeTeaching = list2
                });
                sch = new WeekSchedule();
                sch.AddHoursAllDays(12, 15);
                _blimp.AddTester(new Tester(324040443, "Maoz,", "Shectman")
                {
                    BirthDate = new DateTime(1970, 1, 1), Address = new Address("תל אביב, נס ציונה, ישראל"),
                    Schedule = sch
                });
                sch = new WeekSchedule();
                sch.AddHoursAllDays(12, 15);
                _blimp.AddTester(new Tester(323873182, "Eliran", "Franko")
                {
                    BirthDate = new DateTime(1961, 1, 1), Address = new Address("רחוב הנשיא, קרית שמונה, ישראל"),
                    Schedule = sch
                });
                sch = new WeekSchedule();
                sch.AddHoursAllDays(12, 15);
                _blimp.AddTester(new Tester(323082321, "David", "Arbiv")
                {
                    BirthDate = new DateTime(1950, 1, 1), Address = new Address("חולות גאולים, קדימה צורן, ישראל"),
                    Schedule = sch
                });

                var test1 = new Test(328729660, 319185997)
                {
                    TestTime = new DateTime(2018, 12, 18, 12, 00, 00),
                    AddressOfBeginningTest = new Address("הועד הלאומי 14, ירושלים, ישראל"),
                    LicenseType = LicenseType.A
                };
                try
                {
                    //  test1.SetRouteAndAddressToTest(new Address("jerusalem"));
                }
                catch
                {
                    // ignored
                }

                _blimp.AddTest(test1);
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message,ex.ToString());
            }
        }

        /// <summary>
        ///     check the id .if it is correct the enable the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterIDTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tools.CheckID_IL(uint.Parse(TesterIDTestBox.Text)))
                {
                    TesterLoginButton.IsEnabled = true;
                    TesterIDTestBox.BorderBrush = Brushes.LightGray;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TesterLoginButton.IsEnabled = false;
                TesterIDTestBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        ///     check the id .if it is correct the enable the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeIDTestBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Tools.CheckID_IL(uint.Parse(TraineeIDTestBox.Text)))
                {
                    TraineeLoginButton.IsEnabled = true;
                    TraineeIDTestBox.BorderBrush = Brushes.LightGray;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TraineeLoginButton.IsEnabled = false;
                TraineeIDTestBox.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        ///     On login click open trainee window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TraineeLoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                var win = new TraineeWin(int.Parse(TraineeIDTestBox.Text));
                win.ShowDialog();
                Show();
                TraineeIDTestBox.Text = "";
                TraineeIDTestBox.BorderBrush = Brushes.LightGray;
                TraineeIDTestBox.Focus();
            }
            catch
            {
                ExceptionMessage.Show("Trainee doesn't exist please contact the administrator.");
                Show();
            }
        }

        /// <summary>
        ///     On login click open tester window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TesterLoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                var win = new TesterWin(int.Parse(TesterIDTestBox.Text));
                win.ShowDialog();
                Show();
                TesterIDTestBox.Text = "";
                TesterIDTestBox.BorderBrush = Brushes.LightGray;
                TesterIDTestBox.Focus();
            }
            catch
            {
                ExceptionMessage.Show("Tester doesn't exist please contact the administrator.");
                Show();
            }
        }

        /// <summary>
        ///     On login Click open administrator window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdminLoginButton_Click(object sender, EventArgs e)
        {
            if (AdminUsernameTextBox.Text == Configuration.AdminUser &&
                AdminPasswordTextBox.Password == Configuration.AdminPassword)
            {
                AdminUsernameTextBox.Text = "";
                AdminPasswordTextBox.Password = "";
                AdminUsernameTextBox.Focus();
                Hide();
                var win = new Administrator();
                win.ShowDialog();
                Show();
            }
            else
            {
                ExceptionMessage.Show("Wrong Password or UserName.");
            }
        }

        //Login on Key enter pressed
        private void TabItem_KeyDown_Admin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (AdminLoginButton.IsEnabled)
                {
                    AdminLoginButton_Click(this,new EventArgs());
                }
            }
        }

        //Login on Key enter pressed
        private void TabItem_KeyDown_Tester(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TesterLoginButton.IsEnabled)
                {
                    TesterLoginButton_Click(this, new EventArgs());
                }
            }
        }

        //Login on Key enter pressed
        private void TabItem_KeyDown_Trainee(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TraineeLoginButton.IsEnabled)
                {
                    TraineeLoginButton_Click(this, new EventArgs());

                }
            }
        }
    }
    
}