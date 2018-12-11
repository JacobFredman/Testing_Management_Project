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
using BL;

namespace PLWPF.Admin
{
    /// <summary>
    /// Interaction logic for Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        private IBL bL = FactoryBl.GetObject;
        public Administrator()
        {
            InitializeComponent();
            ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);

            ComboxUpdateTester.ItemsSource = bL.AllTesters.Select(x => x.Id);
            ComboxRemoveTsster.ItemsSource = bL.AllTesters.Select(x => x.Id);

           
            
        }

        private void AddTrainee_Click(object sender, RoutedEventArgs e)
        {
            ManageTrainee.AddTrainee win = new ManageTrainee.AddTrainee();
            win.ShowDialog();
            ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);

        }

        private void Updatetrainee_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ManageTrainee.AddTrainee win = new ManageTrainee.AddTrainee(uint.Parse(ComboxUpdateTrainee.SelectedItem.ToString()));
                win.ShowDialog();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Removetrainee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTrainee(bL.AllTrainee.First(x => x.Id == uint.Parse(ComboxRemoveTrainee.SelectedItem.ToString())));
                ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);

            }
            catch { }
        }

        private void UpdateTester_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ManageTester.AddTester win = new ManageTester.AddTester(uint.Parse(ComboxUpdateTester.SelectedItem.ToString()));
                win.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RemoveTester_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTester(bL.AllTesters.First(x => x.Id == uint.Parse(ComboxRemoveTsster.SelectedItem.ToString())));
                ComboxUpdateTester.ItemsSource = bL.AllTesters.Select(x => x.Id);
                ComboxRemoveTsster.ItemsSource = bL.AllTesters.Select(x => x.Id);
            }
            catch { }
        }

        private void AddTester_Click(object sender, RoutedEventArgs e)
        {
            ManageTester.AddTester win = new ManageTester.AddTester();
            win.ShowDialog();
            ComboxUpdateTester.ItemsSource = bL.AllTesters.Select(x => x.Id);
            ComboxRemoveTsster.ItemsSource = bL.AllTesters.Select(x => x.Id);

        }

        private void GetAllTrainees_Click(object sender, RoutedEventArgs e)
        {
            TestLists.ShowList win = new TestLists.ShowList();
            win.Show();
            win.ShowListInGrade(bL.AllTrainee);
        }

        private void PickTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GetAddressTextBox.Text = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).Address.ToString();
                testlicense.ItemsSource = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).LicenseTypeLearning.Select(x => x.License);
            }
            catch { }
        }

        private void SetTestButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                uint idtester = 0;
            try
            {
               idtester = bL.GetRecommendedTesters((DateTime)TestDatePick.SelectedDate, new BE.Routes.Address(GetAddressTextBox.Text), (BE.LicenseType)testlicense.SelectedItem).First().Id;
            }
            catch(Exception ex)
            {
                throw new Exception("Couldt find tester: "+ex.Message);
            }

                BE.MainObjects.Test test = new BE.MainObjects.Test(idtester, uint.Parse(PickTrainee.SelectedItem.ToString()))
                {
                    TestTime = (DateTime)TestDatePick.SelectedDate,
                    LicenseType = (BE.LicenseType)testlicense.SelectedItem
                };
                if (GetAddressTextBox.Text == "")
                    throw new Exception("Please enter test address");
                    test.SetRouteAndAddressToTest(new BE.Routes.Address(GetAddressTextBox.Text));
                bL.AddTest(test);
                GetTestId.ItemsSource = bL.AllTests.Select(x => x.Id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void ShowTest_Click(object sender, RoutedEventArgs e)
        {
            ManageTest.ShowTest win = new ManageTest.ShowTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
            win.ShowDialog();
        }
    }
}
