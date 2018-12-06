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
using BE;
using BE.Routes;
using BE.MainObjects;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for TraineeWin.xaml
    /// </summary>
    public partial class TraineeWin : Window
    {
        private IBL _blimp = FactoryBl.GetObject;
        private Trainee _trainee;
        public TraineeWin(int id)
        {
            InitializeComponent();
            try
            {
                _trainee = _blimp.AllTrainee.First(x => x.Id == id);
                _textbox.Text = _trainee.ToString();               
            }
            catch
            {
                Close();
            }
        }
    }
}
