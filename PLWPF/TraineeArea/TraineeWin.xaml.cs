using System.Linq;
using System.Windows;
using BE.MainObjects;
using BL;
using PLWPF.TraineeArea;

namespace PLWPF.TraineeArea
{
    /// <inheritdoc />
    /// <summary>
    ///     Interaction logic for TraineeWin.xaml --it is still emty--
    /// </summary>
    public partial class TraineeWin : Window
    {
        private readonly IBL _blimp = FactoryBl.GetObject;
        private readonly Trainee _trainee;

        public TraineeWin(int id)
        {
            InitializeComponent();
            try
            {
                _trainee = _blimp.AllTrainees.First(x => x.Id == id);
                  testDataGrid.DataContext = _blimp.AllTests.Where(x => x.TraineeId == _trainee.Id);
            }
            catch
            {
                Close();
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var win2 = new AddTrainee();
            win2.Show();
           // this.Close();
        }
    }
}