using System.Windows;
using BE.MainObjects;
using BL;
using MahApps.Metro.Controls;

namespace PLWPF.Terter
{
    /// <summary>
    ///     Interaction logic for ShowTest.xaml
    /// </summary>
    public partial class ShowTest : MetroWindow
    {
        /// <summary>
        ///     the test
        /// </summary>
        private readonly Test _test;

        /// <summary>
        ///     Show test details
        /// </summary>
        /// <param name="test"></param>
        public ShowTest(Test test)
        {
            InitializeComponent();
            //set data context
            _test = test;
            DataContext = _test;

            //enable or disable route button
            if (_test.RouteUrl == null) routeUrlButton.IsEnabled = false;
        }

        /// <summary>
        ///     Show route
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteUrlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Routes.ShowUrlInChromeWindow(_test.RouteUrl);
            }
            catch
            {
                // ignored
            }
        }
    }
}