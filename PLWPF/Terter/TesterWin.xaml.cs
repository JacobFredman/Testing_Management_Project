using System.Linq;
using System.Windows;
using BE.MainObjects;
using BL;

namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for TesterWin.xaml --- it is still empty ---
    /// </summary>
    public partial class TesterWin : Window
    {
        private readonly IBL _blimp = FactoryBl.GetObject;
        private readonly Tester _tester;

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