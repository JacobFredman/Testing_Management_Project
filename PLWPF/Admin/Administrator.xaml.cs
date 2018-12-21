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
using PLWPF.Admin.ManageTest;
using PLWPF.Admin.ManageTester;
using PLWPF.Admin.ManageTrainee;

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

            RefreshData();
            TraineeGrid.DataContext = bL.AllTrainees;
            TesterGrid.DataContext = bL.AllTesters;
            TestGrid.DataContext = bL.AllTests;


        }
        #region Trainee

        /// <summary>
        /// Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
            var win = new AddTrainee((TraineeGrid.SelectedItem as Trainee).Id);
            win.ShowDialog();
            RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected trainee 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTrainee((TraineeGrid.SelectedItem as Trainee));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTraineeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddTrainee();
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Tester

        /// <summary>
        /// Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddTester((TesterGrid.SelectedItem as Tester).Id);
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                if(ex.Message!= "Object reference not set to an instance of an object.")
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected trainee 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTester((TesterGrid.SelectedItem as Tester));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTesterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new AddTester();
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Test

        /// <summary>
        /// Update selected Trainee in a new window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new EditTest((TestGrid.SelectedItem as Test).Id);

                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Remove selected trainee 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bL.RemoveTest((TestGrid.SelectedItem as Test));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Add new trainee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTestClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new EditTest();
                win.ShowDialog();
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        /// <summary>
        /// refresh all Data Context
        /// </summary>
        private void RefreshData()
        {
            DataContext = null;
            DataContext = bL;

            TraineeGrid.DataContext = null;
            TraineeGrid.DataContext = bL.AllTrainees;

            TesterGrid.DataContext = null;
            TesterGrid.DataContext = bL.AllTesters;

            TestGrid.DataContext = null;
            TestGrid.DataContext = bL.AllTests;

            NumberOfTraineesLabel.Content = bL.AllTrainees.Count().ToString();
            NumberOfTestersLabel.Content = bL.AllTesters.Count().ToString();
            NumberOfTestsLabel.Content = bL.AllTests.Count().ToString();

        }

        private void TextBoxSearchTrainee_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxSearchTrainee.Text == "")
                TraineeGrid.DataContext = bL.AllTrainees.ToList();
            else
            {
                TraineeGrid.DataContext = bL.SearchTrainee(TextBoxSearchTrainee.Text);
            }
        }

        private void SearchTextBoxTester_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBoxTester.Text == "")
                TesterGrid.DataContext = bL.AllTesters.ToList();
            else
            {
                TesterGrid.DataContext = bL.SearchTester(SearchTextBoxTester.Text);
            }
        }

        private void TextBoxSearchTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxSearchTest.Text == "")
                TestGrid.DataContext = bL.AllTests.ToList();
            else
            {
                TestGrid.DataContext = bL.SearchTest(TextBoxSearchTest.Text);
            }
        }

        private void SearchTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (TextBoxSearchIdTrainee.Text != "") ? TextBoxSearchIdTrainee.Text : null;
            string FName= (TextBoxSearchFirstNameTrainee.Text != "") ? TextBoxSearchFirstNameTrainee.Text : null;
            string LName = (TextBoxSearchLastNameTrainee.Text != "") ? TextBoxSearchLastNameTrainee.Text : null;
            var list = bL.AllTrainees.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (FName != null && FName.ToLower() == p.FirstName.ToLower()) return true;
                if (LName != null && LName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            TraineeGrid.DataContext = list;
        }

        private void SearchClearTraineeButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void SearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (TextBoxSearchIdTester.Text != "") ? TextBoxSearchIdTester.Text : null;
            string FName = (TextBoxSearchFirstNameTester.Text != "") ? TextBoxSearchFirstNameTester.Text : null;
            string LName = (TextBoxSearchLastNameTester.Text != "") ? TextBoxSearchLastNameTester.Text : null;
            var list = bL.AllTesters.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (FName != null && FName.ToLower() == p.FirstName.ToLower()) return true;
                if (LName != null && LName.ToLower() == p.LastName.ToLower()) return true;
                return false;
            });
            TesterGrid.DataContext = list;
        }

        private void ClearSearchTesterButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void SearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (TextBoxSearchIdTest.Text != "") ? TextBoxSearchIdTest.Text : null;
            string traineId = (TextBoxSearchTraineeIDTest.Text != "") ? TextBoxSearchTraineeIDTest.Text : null;
            string testerId = (TextBoxSearchTesterIDTest.Text != "") ? TextBoxSearchTesterIDTest.Text : null;
            var list = bL.AllTests.Where(p =>
            {
                if (id != null && id == p.Id.ToString()) return true;
                if (traineId != null && traineId.ToLower() == p.TraineeId.ToString().ToLower()) return true;
                if (testerId != null && testerId.ToLower() == p.TesterId.ToString().ToLower()) return true;
                return false;
            });
            TestGrid.DataContext = list;
        }

        private void ClaerSearchTestButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
    }
}
