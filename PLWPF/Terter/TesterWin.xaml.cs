using System;
using System.Linq;
using System.Threading;
using System.Windows;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;
using PLWPF.Nofitications;
using PLWPF.Terter;

namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for TesterWin.xaml --- it is still empty ---
    /// </summary>
    public partial class TesterWin : MetroWindow
    {
        private readonly IBL _blimp = FactoryBl.GetObject;
        private readonly Tester _tester;

        public TesterWin(int id)
        {
            InitializeComponent();
            try
            {
                //initialize window
                _tester = _blimp.AllTesters.First(x => x.Id == id);
                TextBoxHi.Content = "Welcome " + _tester.FirstName + " " + _tester.LastName;
                Refresh();
            }
            catch
            {
                Close();
            }
        }

        //refresh grid content
        private void Refresh()
        {
            TestToDoGrid.DataContext =
                FactoryBl.GetObject.AllTests.Where(x => x.TesterId == _tester.Id && x.TestTime >=DateTime.Now);
            TestToUpdateGrid.DataContext =
                FactoryBl.GetObject.AllTests.Where(x => x.TesterId == _tester.Id && x.TestTime <= DateTime.Now);

        }

        //on test to do grid double click
        private void TestToDoGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var win = new ShowTest((Test) TestToDoGrid.SelectedItem);
                win.ShowDialog();

            }
            catch { }
        }

        //on test to update grid double click
        private void TestToUpdateGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var test = TestToUpdateGrid.SelectedItem as Test;
                var win = new UpdateTest(test);
                win.ShowDialog();
                var passed = test.Passed;


                Refresh();

                //get trainee and updated test
                var trainee = FactoryBl.GetObject.AllTrainees.First(x => x.Id == test.TraineeId);
                test = FactoryBl.GetObject.AllTests.First(x => x.Id == test.Id);

                if (test.Passed == passed)
                    return;

                if (string.IsNullOrEmpty(trainee.EmailAddress))
                {
                    return;
                }

                //Send Email
                var thread = new Thread(() =>
                {
                    try
                    {
                        Pdf.CreateLicensePdf(test, trainee);
                        Email.SentEmailToTraineeAfterTest(test, trainee);

                    
                    }
                    catch 
                    {
                  
                    }



                });
                thread.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                    ExceptionMessage.Show(ex.Message, ex.ToString());
            }

        }

        //refresh button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}