using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Nofitications;
using PLWPF.TraineeArea;


namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for TraineeWin.xaml 
    /// </summary>
    public partial class TraineeWin : MetroWindow
    {
        /// <summary>
        /// Bl object
        /// </summary>
        private readonly IBL _blimp = FactoryBl.GetObject;

        /// <summary>
        /// The trainee
        /// </summary>
        private readonly Trainee _trainee;

        /// <summary>
        /// Trainee window
        /// </summary>
        /// <param name="id"></param>
        public TraineeWin(int id)
        {
            InitializeComponent();
            try
            {
                //try to add trainee
                _trainee = _blimp.AllTrainees.First(x => x.Id == id);
                TextBoxHi.Content = "Welcome " +_trainee.FirstName+" "+_trainee.LastName;
                Refresh();
            }
            catch
            {
                Close();
            }
        }

        /// <summary>
        /// refresh grid content
        /// </summary>
        private void Refresh()
        {
            TestToDoGrid.DataContext =
                FactoryBl.GetObject.AllTests.Where(x => x.TraineeId == _trainee.Id && x.TestTime >= DateTime.Now);
            TestToUpdateGrid.DataContext =
                FactoryBl.GetObject.AllTests.Where(x => x.TraineeId == _trainee.Id && x.TestTime <= DateTime.Now);

        }

        /// <summary>
        /// Add new test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetTest_Button_Click(object sender, RoutedEventArgs e)
        {
            (new Thread(() =>
            {
                try
                {
                    //Check internet connectivity
                    try
                    {
                        var wc = new WebClient();
                        wc.DownloadData("https://www.google.com/");
                    }
                    catch { throw new Exception("There is No Internet Connection.Please Try Again Later."); }

                    //open window
                    void Act1()
                    {
                        var win = new EditTest(_trainee);
                        win.ShowDialog();
                        Refresh();
                    }

                    Dispatcher.BeginInvoke((Action)Act1);
                }
                catch (Exception ex)
                {
                    void Act2()
                    {
                        ExceptionMessage.Show(ex.Message, ex.ToString());
                    }

                    Dispatcher.BeginInvoke((Action)Act2);
                }
            })).Start();
        }

  
        /// <summary>
        /// Refresh data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Show test results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestToUpdateGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var win = new ShowTestResoults((Test) TestToUpdateGrid.SelectedItem);
                win.ShowDialog();
            }
            catch
            {
                // ignored
            }
        }
    }
}