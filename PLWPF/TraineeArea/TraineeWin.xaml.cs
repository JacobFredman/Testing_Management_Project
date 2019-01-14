using System;
using System.Linq;
using System.Windows;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;
using PLWPF.TraineeArea;


namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for TraineeWin.xaml --it is still emty--
    /// </summary>
    public partial class TraineeWin : MetroWindow
    {
        private readonly IBL _blimp = FactoryBl.GetObject;
        private readonly Trainee _trainee;

        public TraineeWin(int id)
        {
            InitializeComponent();
            try
            {
                _trainee = _blimp.AllTrainees.First(x => x.Id == id);
                TextBoxHi.Content = "Welcome " +_trainee.FirstName+" "+_trainee.LastName;
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
                FactoryBl.GetObject.AllTests.Where(x => x.TraineeId == _trainee.Id && x.TestTime >= DateTime.Now);
            TestToUpdateGrid.DataContext =
                FactoryBl.GetObject.AllTests.Where(x => x.TraineeId == _trainee.Id && x.TestTime <= DateTime.Now);

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
         EditTest editTest = new EditTest(_trainee); 
            editTest.ShowDialog();
            Refresh();
        }

  

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}