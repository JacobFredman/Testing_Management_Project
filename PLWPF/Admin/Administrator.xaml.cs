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
using System.Threading;

namespace PLWPF.Admin
{
    /// <summary>
    /// The administrator Window
    /// </summary>
    public partial class Administrator : Window
    {
        //a BL object
        private IBL bL = FactoryBl.GetObject;

        public Administrator()
        {
            InitializeComponent();

            #region Set Comboxes Source

            //set the trainee combox source
            ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);

            //set the tester combox
            ComboxUpdateTester.ItemsSource = bL.AllTesters.Select(x => x.Id);
            ComboxRemoveTsster.ItemsSource = bL.AllTesters.Select(x => x.Id);

            //set dfualt dates
            PickDateTester.SelectedDate = DateTime.Now.Date;
            TestDatePick.SelectedDate = DateTime.Now.Date;

            //set source for time picker in test
            var TimesList = new List<string>();
            for(int i = 0; i < 24; i++)
            {
                TimesList.Add(string.Format("{0:00}:00", i));
            }
            SelectTimeTest.ItemsSource = TimesList;
            SelectTimeTest.SelectedItem = "12:00";

            //set item source for check func bl combox
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
 
            #endregion

        }

        #region Manage Trainee

        /// <summary>
        /// On Add Trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTrainee_Click(object sender, RoutedEventArgs e)
        {
            //open the add trainee window
            ManageTrainee.AddTrainee win = new ManageTrainee.AddTrainee();
            win.ShowDialog();

            //update the comboxes
            ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
            PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);

        }

        /// <summary>
        /// On update trainee click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Updatetrainee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if there is a trainee selected the open update trainee window
                if (ComboxUpdateTrainee.SelectedItem == null) throw new Exception("Please select a trainee");
                ManageTrainee.AddTrainee win = new ManageTrainee.AddTrainee(uint.Parse(ComboxUpdateTrainee.SelectedItem.ToString()));
                win.ShowDialog();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// On remove trainee click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Removetrainee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if trainee selcted the remove trainee
                if(ComboxRemoveTrainee.SelectedItem==null) throw new Exception("Please select a trainee");
                bL.RemoveTrainee(bL.AllTrainee.First(x => x.Id == uint.Parse(ComboxRemoveTrainee.SelectedItem.ToString())));

                //update comboxes
                ComboxUpdateTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                ComboxRemoveTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);
                PickTrainee.ItemsSource = bL.AllTrainee.Select(x => x.Id);

            }
            catch { }
        }
        #endregion

        #region Manage Testers
        /// <summary>
        /// On update tester click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTester_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if tester is selected then open add tester window
                if(ComboxUpdateTester.SelectedItem==null) throw new Exception("Please select a tester");
                ManageTester.AddTester win = new ManageTester.AddTester(uint.Parse(ComboxUpdateTester.SelectedItem.ToString()));
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// On remove tester click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTester_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if tester is selected then open remove tester window
                if (ComboxRemoveTsster.SelectedItem == null) throw new Exception("Please select a tester");
                bL.RemoveTester(bL.AllTesters.First(x => x.Id == uint.Parse(ComboxRemoveTsster.SelectedItem.ToString())));
                //update combox
                ComboxUpdateTester.ItemsSource = bL.AllTesters.Select(x => x.Id);
                ComboxRemoveTsster.ItemsSource = bL.AllTesters.Select(x => x.Id);
            }
            catch { }
        }

        /// <summary>
        /// on Add tester click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTester_Click(object sender, RoutedEventArgs e)
        {
            //open add tester window
            ManageTester.AddTester win = new ManageTester.AddTester();
            win.ShowDialog();

            //update comboxes
            ComboxUpdateTester.ItemsSource = bL.AllTesters.Select(x => x.Id);
            ComboxRemoveTsster.ItemsSource = bL.AllTesters.Select(x => x.Id);
        }

        #endregion

        #region Manage Tests
        /// <summary>
        /// On select trainee in set test changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickTrainee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //set recommended test details
                GetAddressTextBox.Text = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).Address.ToString();
                testlicense.ItemsSource = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).LicenseTypeLearning.Select(x => x.License);
                testlicense.SelectedItem = bL.AllTrainee.First(x => x.Id == uint.Parse(PickTrainee.SelectedItem.ToString())).LicenseTypeLearning.Select(x => x.License).First();
            }
            catch { }
        }

        /// <summary>
        /// On set test click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (PickTrainee.SelectedItem == null) throw new Exception("Please select a trainee");
            DateTime Date = (DateTime)TestDatePick.SelectedDate;
            var time = double.Parse(SelectTimeTest.SelectedItem.ToString().Substring(0, 2));
            var address = new BE.Routes.Address(GetAddressTextBox.Text);
            var license = (BE.LicenseType)testlicense.SelectedItem;
            var traineeId = uint.Parse(PickTrainee.SelectedItem.ToString());

            //disable test group
            TestGroup.IsEnabled = false;
            testAdding.Visibility = Visibility.Visible;

            //run it in a prosses becouse it can take time
            (new Thread(() =>
            {
                try
                {


                    uint idtester = 0;

                    //try to get a tester 
                    try
                    {
                        if ((int)Date.DayOfWeek > 4) throw new Exception("Testers don't work on " + Date.DayOfWeek.ToString());


                        //DateTime Date = (DateTime)TestDatePick.SelectedDate;
                        Date = Date.AddHours(time);
                        idtester = bL.GetRecommendedTesters(Date, address, license).First().Id;
                    }
                    catch (Exception ex)
                    {                                               //for debugging
                        throw new Exception("Couldn't find tester: " + ex.Message);
                    }

                    //create a new test
                    Test test = new Test(idtester, traineeId)
                    {
                        TestTime = Date,
                        LicenseType = license
                    };
                    //if the address is empty
                    if (address.ToString() == "")
                        throw new Exception("Please enter test address");
                    //try to find a route for the test
                    try
                    {
                        test.SetRouteAndAddressToTest(address);
                    }
                    catch
                    {
                        test.AddressOfBeginningTest = address;
                    }
                    //add the test
                    bL.AddTest(test);

                    //update combox
                    Action action = () => GetTestId.ItemsSource = bL.AllTests.Select(x => x.Id);
                    Dispatcher.BeginInvoke(action);

                    //enable test group
                    action = () => { TestGroup.IsEnabled = true; testAdding.Visibility = Visibility.Hidden; };
                    Dispatcher.BeginInvoke(action);

                    //update tester id in trainee
                    var trainee = bL.AllTrainee.First(x => x.Id == test.TraineeId);
                    trainee.TesterId = test.TesterId.ToString();
                    bL.UpdateTrainee(trainee);

                    MessageBox.Show("test added succecfuly.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //enable test group
                    Action action = () => { TestGroup.IsEnabled = true; testAdding.Visibility = Visibility.Hidden; };
                    Dispatcher.BeginInvoke(action);
                }
            })).Start();

        }

        /// <summary>
        /// On show test click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //show test in new window
                if (GetTestId.SelectedItem == null) throw new Exception("Please select a trainee");
                ManageTest.ShowTest win = new ManageTest.ShowTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// On update test click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //open update test window
                if (GetTestId.SelectedItem == null) throw new Exception("Please select a trainee");
                var win = new ManageTest.UpdateTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// On remove test click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GetTestId.SelectedItem == null) throw new Exception("Please select a trainee");
                //remove test
                bL.RemoveTest(bL.AllTests.First(x => x.Id == GetTestId.Text));
                //update combox
                GetTestId.ItemsSource = bL.AllTests.Select(x => x.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Check BL Functions
        /// <summary>
        /// On show lists click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TestLists.ShowList win;
                //on selcted function open a new window with a list of the data
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
                    
                        //grouping functions
                    case "GetAllTestsByLicense":
                        win = new TestLists.ShowList(bL.GetAllTestsByLicense().First(x => x.Key == (BE.LicenseType)GroupingBox.SelectedItem));
                        win.ShowDialog();
                        break;

                    case "GetAllTraineesByLicense":
                        win = new TestLists.ShowList(bL.GetAllTraineesByLicense().First(x => String.Join(" ", x.Key)
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

        /// <summary>
        /// On selected function change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFuncBl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //set the window element according to the function
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
                        PickDateTester.Visibility = Visibility.Collapsed;
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

                    //grouging functions
                    case "GetAllTestsByLicense":
                        GroupingBox.ItemsSource = bL.GetAllTestsByLicense().Select(x => x.Key);
                        PickDateTester.Visibility = Visibility.Collapsed;
                        AddressLebel.Visibility = Visibility.Collapsed;
                        AddressTextBox.Visibility = Visibility.Collapsed;
                        RadoisLabel.Visibility = Visibility.Collapsed;
                        Radios.Visibility = Visibility.Collapsed;
                        GroupingBox.Visibility = Visibility.Visible;
                        break;

                    case "GetAllTraineesByLicense":
                        GroupingBox.ItemsSource = bL.GetAllTraineesByLicense().Select(x => String.Join(" ", x.Key)).Distinct();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Export to excel

        /// <summary>
        /// Export all trainees to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllTraineeToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                disableExportGroup();
                (new Thread(() => {
                    bL.AllTrainee.ToExcel();
                    enableExportGroup();
                })).Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                enableExportGroup();
            }
        }

        /// <summary>
        /// expport all testers to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllTestersToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                disableExportGroup();
                (new Thread(() => {
                    bL.AllTesters.ToExcel();
                    enableExportGroup();
                })).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                enableExportGroup();
            }
        }

        /// <summary>
        /// export all test to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllTestsToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                disableExportGroup();
                (new Thread(() => {
                    bL.AllTests.ToExcel();
                    enableExportGroup();
                })).Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                enableExportGroup();
            }
        }

        /// <summary>
        /// disable the export group
        /// </summary>
        private void disableExportGroup()
        {
            if (!CheckAccess())
            {
                Action action = disableExportGroup;
                Dispatcher.BeginInvoke(action);
            }
            else
            {
                ExportGroup.IsEnabled = false;
                exportingLabel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// enable the export group
        /// </summary>
        private void enableExportGroup()
        {
            if (!CheckAccess())
            {
                Action action = enableExportGroup;
                Dispatcher.BeginInvoke(action);
            }
            else
            {
                ExportGroup.IsEnabled = true;
                exportingLabel.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region Send email
        /// <summary>
        /// Send email before test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendBeforeTest_Click(object sender, RoutedEventArgs e)
        {
            disableEmailtGroup();
            (new Thread(() =>
            {
                var number = bL.SendEmailToAllTraineeBeforeTest();
                MessageBox.Show("You sended " + number + " emails.");
                enableEmailtGroup();
            })).Start();
        }

        /// <summary>
        /// send email after test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendAfterTest_Click(object sender, RoutedEventArgs e)
        {
            disableEmailtGroup();
            (new Thread(() =>
            {
                var number = bL.SendEmailToAllTraineeAfterTest();
                MessageBox.Show("You sended " + number + " emails.");
                enableEmailtGroup();
            })).Start();
        }

        /// <summary>
        /// enable the email group
        /// </summary>
        private void enableEmailtGroup()
        {
            if (!CheckAccess())
            {
                Action action = enableEmailtGroup;
                Dispatcher.BeginInvoke(action);
            }
            else
            {
                EmailGroup.IsEnabled = true;
                emailWaitLabel.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// disable the email group
        /// </summary>
        private void disableEmailtGroup()
        {
            if (!CheckAccess())
            {
                Action action = disableEmailtGroup;
                Dispatcher.BeginInvoke(action);
            }
            else
            {
                EmailGroup.IsEnabled = false;
                emailWaitLabel.Visibility = Visibility.Visible;
            }
        }
        #endregion

    }
}
