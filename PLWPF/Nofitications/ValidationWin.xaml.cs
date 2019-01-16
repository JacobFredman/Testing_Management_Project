using System.Windows;
using MahApps.Metro.Controls;

namespace PLWPF.Nofitications
{
    /// <summary>
    ///     A validation message
    /// </summary>
    public static class ValidationMessage
    {
        /// <summary>
        ///     the returned value
        /// </summary>
        public static bool ReturnedYes;

        /// <summary>
        ///     Show a validation message
        /// </summary>
        /// <param name="message">message to show</param>
        /// <returns></returns>
        public static bool Show(string message)
        {
            var win = new ValidationWin(message);
            win.ShowDialog();
            return ReturnedYes;
        }
    }

    /// <summary>
    ///     Interaction logic for ValidationWin.xaml
    /// </summary>
    public partial class ValidationWin : MetroWindow
    {
        public ValidationWin(string message)
        {
            InitializeComponent();
            //set text
            TextBlock.Text = message;
        }

        /// <summary>
        ///     On yes click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Yes(object sender, RoutedEventArgs e)
        {
            ValidationMessage.ReturnedYes = true;
            Close();
        }

        /// <summary>
        ///     On no click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            ValidationMessage.ReturnedYes = false;
            Close();
        }
    }
}