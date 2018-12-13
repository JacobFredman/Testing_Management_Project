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
    /// Interaction logic for TesterWin.xaml --- it is still empty ---
    /// </summary>
    public partial class TesterWin : Window
    {
        private IBL _blimp = FactoryBl.GetObject;
        private Tester _tester;
        public TesterWin(int id)
        {
            InitializeComponent();
            try
            {
                _tester = _blimp.AllTesters.First(x => x.Id == id);
                _textBox.Text = _tester.ToString();
            }
            catch
            {
                Close();
            }
        }
    }
}
