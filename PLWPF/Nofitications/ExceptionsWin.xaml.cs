using System.Windows;
using MahApps.Metro.Controls;

namespace PLWPF.Nofitications
{
    internal static class ExceptionMessage
    {
        public static void Show(string Message, string Details = "")
        {
            var win = new ExceptionsWin(Message, Details);
            win.ShowDialog();
        }
    }

    /// <summary>
    ///     Interaction logic for ExceptionsWin.xaml
    /// </summary>
    public partial class ExceptionsWin : MetroWindow
    {
        public ExceptionsWin(string Message, string Details = "")
        {
            InitializeComponent();
            TextBlockMessage.Text = Message;
            TextBoxDetails.Text = Details;
            if (Details == "") Expander.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}