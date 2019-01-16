using System.Windows;
using MahApps.Metro.Controls;

namespace PLWPF.Nofitications
{
    /// <summary>
    ///     Show Message in Metro style
    /// </summary>
    internal static class ExceptionMessage
    {
        /// <summary>
        ///     Show Message in Metro style
        /// </summary>
        /// <param name="message">Main message</param>
        /// <param name="details">Details</param>
        public static void Show(string message, string details = "")
        {
            var win = new ExceptionsWin(message, details);
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
            //set the text
            TextBlockMessage.Text = Message;
            TextBoxDetails.Text = Details;

            //remove the details it they are emoty
            if (Details == "") Expander.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     On click ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}