using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    static class ExceptionMessage
    {
        public static void Show(string Message, string Details="")
        {
            
             
            var win=new ExceptionsWin(Message, Details);
            win.ShowDialog();
            
        }
    }

    /// <summary>
    /// Interaction logic for ExceptionsWin.xaml
    /// </summary>
    public partial class ExceptionsWin : MetroWindow
    {
        public ExceptionsWin(string Message, string Details = "")
        {
            InitializeComponent();
            TextBlockMessage.Text = Message;
            TextBoxDetails.Text = Details;
            if (Details == "")
            {
                Expander.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    
    }
}
