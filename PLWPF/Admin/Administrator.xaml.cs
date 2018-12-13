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
using BE.MainObjects;
using BE;


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

            PickDateTester.SelectedDate = DateTime.Now.Date;
            TestDatePick.SelectedDate = DateTime.Now.Date;
            var TimesList = new List<string>();
            for(int i = 0; i < 24; i++)
            {
                TimesList.Add(string.Format("{0:00}:00", i));
            }
            SelectTimeTest.ItemsSource = TimesList;
            SelectTimeTest.SelectedItem = "12:00";

            var funcList = new List<string>();
            funcList.Add("AllTesters");
            funcList.Add("AllTests");
            funcList.Add("AllTrainees");
            funcList.Add("GetAvailableTesters");
            funcList.Add("GetAllTestersInRadios");
            funcList.Add("GetAllTestsSortedByDate");
            funcList.Add("GetAllTestInMonth");
            funcList.Add("GetAllTestInDay");
            funcList.Add("GetAllTestsToCome");
            funcList.Add("GetAllTestsThatHappened");
            funcList.Add("GetAllTraineeThatPassedToday");
            funcList.Add("GetAllTraineeThatDidNotPassedToday");
            funcList.Add("GetAllTestsByLicense");
            funcList.Add("GetAllTraineesByLicense");
            funcList.Add("GetAllTestersByLicense");
            funcList.Add("GetAllTraineesByTester");
            funcList.Add("GetAllTraineesBySchool");
            funcList.Add("GetAllTraineeByNumberOfTests");
            SelectFuncBl.ItemsSource = funcList;



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
                if (ComboxUpdateTrainee.SelectedItem == null) throw new Exception("Please select a trainee");
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
                if(ComboxRemoveTrainee.SelectedItem==null) throw new Exception("Please select a trainee");
                bL.RemoveTrainee(bL.AllTrainee.First(x => x.Id == uint.Parse(ComboxRemoveTrainee.SelectedItem.ToString())));
                ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                TestDatePick.SelectedDate = DateTime.Now;

            }
            catch { }
        }

        private void UpdateTester_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(ComboxUpdateTester.SelectedItem==null) throw new Exception("Please select a tester");
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
                if (ComboxRemoveTsster.SelectedItem == null) throw new Exception("Please select a tester");
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
           
        }

        private void PickTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GetAddressTextBox.Text = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).Address.ToString();
                testlicense.ItemsSource = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).LicenseTypeLearning.Select(x => x.License);
                PickTrainee.SelectedItem = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).LicenseTypeLearning.Select(x => x.License).First();
            }
            catch { }
        }

        private void SetTestButton_Click(object sender, RoutedEventArgs e)
        {
            try {
                uint idtester = 0;
            try
            {
               DateTime Date = (DateTime)TestDatePick.SelectedDate;
               Date = Date.AddHours(double.Parse(SelectTimeTest.SelectedItem.ToString().Substring(0, 2)));
               idtester = bL.GetRecommendedTesters(Date, new BE.Routes.Address(GetAddressTextBox.Text), (BE.LicenseType)testlicense.SelectedItem).First().Id;
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
                try
                {
                    test.SetRouteAndAddressToTest(new BE.Routes.Address(GetAddressTextBox.Text));
                }
                catch { }
                bL.AddTest(test);

                GetTestId.ItemsSource = bL.AllTests.Select(x => x.Id);
                bL.AllTrainee.First(x => x.Id == test.TraineeId).TesterId = test.TesterId.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void ShowTest_Click(object sender, RoutedEventArgs e)
        {
            ManageTest.ShowTest win = new ManageTest.ShowTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
            win.ShowDialog();
        }

        private void UpdateTest_Click(object sender, RoutedEventArgs e)
        {
            var win = new ManageTest.UpdateTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
            win.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestLists.ShowList win;
                switch (SelectFuncBl.SelectedItem)
                {
                    case "AllTesters":
                        win = new TestLists.ShowList(bL.AllTesters);
                        win.ShowDialog();
                        break;
                    case "AllTests":
                        win = new TestLists.ShowList(bL.AllTests);
                        win.ShowDialog();
                        break;
                    case "AllTrainees":
                        win = new TestLists.ShowList(bL.AllTrainee);
                        win.ShowDialog();
                        break;

                    case "GetAvailableTesters":
                        win = new TestLists.ShowList(bL.GetAvailableTesters((DateTime)PickDateTester.SelectedDate));
                        win.ShowDialog();
                        break;
                    case "GetAllTestersInRadios":
                        win = new TestLists.ShowList(bL.GetAllTestersInRadios(int.Parse(Radios.Text), new BE.Routes.Address(AddressTextBox.Text)));
                        win.ShowDialog();
                        break;
                    case "GetAllTestsSortedByDate":
                        win = new TestLists.ShowList(bL.GetAllTestsSortedByDate());
                        win.ShowDialog();
                        break;
                    case "GetAllTestInMonth":
                        win = new TestLists.ShowList(bL.GetAllTestInMonth((DateTime)PickDateTester.SelectedDate));
                        win.ShowDialog();
                        break;
                    case "GetAllTestInDay":
                        win = new TestLists.ShowList(bL.GetAllTestInDay((DateTime)PickDateTester.SelectedDate));
                        win.ShowDialog();
                        break;
                    case "GetAllTestsToCome":
                        win = new TestLists.ShowList(bL.GetAllTestsToCome());
                        win.ShowDialog();
                        break;
                    case "GetAllTestsThatHappened":
                        win = new TestLists.ShowList(bL.GetAllTestsThatHappened());
                        win.ShowDialog();
                        break;
                    case "GetAllTraineeThatPassedToday":
                        win = new TestLists.ShowList(bL.GetAllTraineeThatPassedToday((DateTime)PickDateTester.SelectedDate));
                        win.ShowDialog();
                        break;
                    case "GetAllTraineeThatDidNotPassedToday":
                        win = new TestLists.ShowList(bL.GetAllTraineeThatDidNotPassedToday((DateTime)PickDateTester.SelectedDate));
                        win.ShowDialog();
                        break;
                    //grouping

                    case "GetAllTestsByLicense":
                        win = new TestLists.ShowList(bL.GetAllTestsByLicense().First(x => x.Key == (BE.LicenseType)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;
                    case "GetAllTraineesByLicense":
                        win = new TestLists.ShowList(bL.GetAllTraineesByLicense().First(x => String.Join(" ", x.Key.Select(y => y.License))
                                == (string)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;
                    case "GetAllTestersByLicense":
                        win = new TestLists.ShowList(bL.GetAllTestersByLicense().First(x => String.Join(" ", x.Key) == (string)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;
                    case "GetAllTraineesByTester":
                        win = new TestLists.ShowList(bL.GetAllTraineesByTester().First(x => x.Key == (string)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;
                    case "GetAllTraineesBySchool":
                        win = new TestLists.ShowList(bL.GetAllTraineesBySchool().First(x => x.Key == (string)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;
                    case "GetAllTraineeByNumberOfTests":
                        win = new TestLists.ShowList(bL.GetAllTraineeByNumberOfTests().First(x => x.Key == (int)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;
                    default:
                        break;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

    
        }

        private void SelectFuncBl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelectFuncBl.SelectedItem)
            {
                case "AllTesters":
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;
                    break;
                case "AllTests":
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;
                    break;
                case "AllTrainees":
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;
                    break;

                case "GetAvailableTesters":
                    PickDateTester.Visibility = Visibility.Visible;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestersInRadios":
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Visible;
                    AddressTextBox.Visibility = Visibility.Visible;
                    RadoisLabel.Visibility = Visibility.Visible;
                    Radios.Visibility = Visibility.Visible;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestsSortedByDate":
                    PickDateTester.Visibility = Visibility.Visible;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestInMonth":
                    PickDateTester.Visibility = Visibility.Visible;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestInDay":
                    PickDateTester.Visibility = Visibility.Visible;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestsToCome":
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestsThatHappened":
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTraineeThatPassedToday":
                    PickDateTester.Visibility = Visibility.Visible;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTraineeThatDidNotPassedToday":
                    PickDateTester.Visibility = Visibility.Visible;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Collapsed;

                    break;
                case "GetAllTestsByLicense":
                    GroupingBox.ItemsSource = bL.GetAllTestsByLicense().Select(x => x.Key);
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Visible;
                    break;
                    //grouging
                case "GetAllTraineesByLicense":
                    GroupingBox.ItemsSource = bL.GetAllTraineesByLicense().Select(x =>String.Join(" ", x.Key.Select(y=>y.License))).Distinct();

                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Visible;

                    break;
                case "GetAllTestersByLicense":
                    GroupingBox.ItemsSource = bL.GetAllTestersByLicense().Select(x => String.Join(" ", x.Key)).Distinct();

                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Visible;

                    break;
                case "GetAllTraineesByTester":
                    GroupingBox.ItemsSource = bL.GetAllTraineesByTester().Select(x => x.Key);
                    GroupingBox.Visibility = Visibility.Visible;
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    break;
                case "GetAllTraineesBySchool":
                    GroupingBox.ItemsSource = bL.GetAllTraineesBySchool().Select(x => x.Key);
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Visible;

                    break;
                case "GetAllTraineeByNumberOfTests":
                    GroupingBox.ItemsSource = bL.GetAllTraineeByNumberOfTests().Select(x => x.Key);
                    PickDateTester.Visibility = Visibility.Collapsed;
                    AddressLebel.Visibility = Visibility.Collapsed;
                    AddressTextBox.Visibility = Visibility.Collapsed;
                    RadoisLabel.Visibility = Visibility.Collapsed;
                    Radios.Visibility = Visibility.Collapsed;
                    GroupingBox.Visibility = Visibility.Visible;

                    break;
                default:
                    break;
            }
        }

        private void RemoveTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
                GetTestId.ItemsSource = bL.AllTests.Select(x => x.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
