using System.Windows;
using BE.MainObjects;
using MahApps.Metro.Controls;

namespace PLWPF.Terter
{
    /// <summary>
    /// Interaction logic for ShowTest.xaml
    /// </summary>
    public partial class ShowTest : MetroWindow
    {
        private Test _test;

        public ShowTest(Test test)
        {
            InitializeComponent();
            _test = test;
            DataContext = _test;
            if (_test.RouteUrl == null)
            {
                routeUrlButton.IsEnabled = false;
            }
        }

        private void RouteUrlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.Routes.ShowUrlInChromeWindow(_test.RouteUrl);
            }
            catch { }
        }
    }
}
