using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BE;
using BE.MainObjects;
using BE.Routes;
using BL;
using PLWPF.Admin;

namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
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

            if (Configuration.firtOpenProgram)
            {
                IdTextBox.Text = "Admin";
                BirthDateTextBox.Text = "admin";
            }

            Configuration.firtOpenProgram = false;
        }

        /// <summary>
        ///     Set a error message if the ID is incorrect
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
        ///     Set a error message if the password is incorrect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BirthDateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var date = DateTime.MaxValue;

                if (BirthDateTextBox.Text != Configuration.AdminPassword &&
                    !DateTime.TryParse(BirthDateTextBox.Text, out date))
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
        ///     Try to login
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
                    var Win = new Administrator();
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
                        var Win = new TesterWin(int.Parse(IdTextBox.Text));
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
                        var Win2 = new TraineeWin(int.Parse(IdTextBox.Text));
                        Hide();
                        Win2.ShowDialog();
                        IdTextBox.Text = "";
                        BirthDateTextBox.Text = "";
                        IdErrorLabel.Visibility = Visibility.Hidden;
                        BirthDateError.Visibility = Visibility.Hidden;
                        Show();
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        ///     for debbuging only !!!!!!!!!!!!!!!!!!!!!!
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
                    {BirthDate = new DateTime(1985, 29, 12)});
                _blimp.AddTrainee(new Trainee(319185971, Gender.Male, "Moshe", "Levi")
                    {BirthDate = new DateTime(1987, 1, 1)});
                _blimp.AddTrainee(new Trainee(314661133, Gender.Male, "Bob", "Ray")
                    {BirthDate = new DateTime(1980, 1, 1)});
                _blimp.AddTrainee(new Trainee(324126747, Gender.Male, "Avi", "Alon")
                    {BirthDate = new DateTime(1970, 1, 1)});
                _blimp.AddTrainee(new Trainee(326591088, Gender.Female, "Avia", "Abu")
                    {BirthDate = new DateTime(2014, 1, 1)});
                _blimp.AddTrainee(new Trainee(342533643, Gender.Male, "Gil", "Rami")
                    {BirthDate = new DateTime(2000, 1, 1)});
                _blimp.AddTrainee(new Trainee(339794166, Gender.Male, "David", "Aboulafia")
                    {BirthDate = new DateTime(2002, 1, 1)});
                _blimp.AddTrainee(new Trainee(336390885, Gender.Male, "Shlomo", "Simchon")
                    {BirthDate = new DateTime(1991, 1, 1)});
                _blimp.AddTrainee(new Trainee(332484609, Gender.Female, "Gavriela", "Abuxsis")
                    {BirthDate = new DateTime(1991, 1, 1)});
                _blimp.AddTrainee(new Trainee(332307065, Gender.Female, "Yafa", "Alaluf")
                    {BirthDate = new DateTime(1999, 1, 1)});
                _blimp.AddTrainee(new Trainee(332270446, Gender.Male, "Dudu", "Tapiro")
                    {BirthDate = new DateTime(2005, 1, 1)});
                _blimp.AddTrainee(new Trainee(329043459, Gender.Male, "Pinchas", "Moshe")
                    {BirthDate = new DateTime(1988, 1, 1)});

                var sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                var list2 = new List<LicenseType>();
                list2.Add(LicenseType.A);
                _blimp.AddTester(new Tester(328729660, "meir", "")
                {
                    BirthDate = new DateTime(1985, 1, 1), Address = new Address("בית שמש"), MaxDistance = 10000000,
                    Schedule = sch, MaxWeekExams = 10, LicenseTypeTeaching = list2
                });
                sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                _blimp.AddTester(new Tester(324040443, "Maoz,", "Shectman")
                    {BirthDate = new DateTime(1977, 1, 1), Address = new Address("Tel Aviv"), Schedule = sch});
                sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                _blimp.AddTester(new Tester(323873182, "Eliran", "Franko")
                    {BirthDate = new DateTime(1982, 1, 1), Address = new Address("Beit shemesh"), Schedule = sch});
                sch = new WeekSchedule();
                sch.AddHoursAllDays(0, 23);
                _blimp.AddTester(new Tester(323082321, "David", "Arbiv")
                    {BirthDate = new DateTime(1992, 1, 1), Address = new Address("Holon"), Schedule = sch});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}