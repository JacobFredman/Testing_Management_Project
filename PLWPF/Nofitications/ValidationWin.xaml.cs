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
using MahApps.Metro.Controls;

namespace PLWPF.Nofitications
{
    public static class ValidationMessage
    {
        public static bool returnedYes = false;
        public static  bool Show(string message)
        {
            var win=new ValidationWin(message);
            win.ShowDialog();
            return returnedYes;
        }
    }

    /// <summary>
    /// Interaction logic for ValidationWin.xaml
    /// </summary>
    public partial class ValidationWin : MetroWindow
    {
        public ValidationWin(string message)
        {
            InitializeComponent();
            TextBlock.Text = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ValidationMessage.returnedYes = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ValidationMessage.returnedYes = false;
            Close();
        }
    }
}
