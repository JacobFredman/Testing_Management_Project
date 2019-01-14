using System;
using System.Linq;
using System.Windows;
using BE;
using BE.MainObjects;
using MahApps.Metro.Controls;
using PLWPF.Nofitications;

namespace PLWPF.Terter
{
    /// <summary>
    /// Interaction logic for UpdateTest.xaml
    /// </summary>
    public partial class UpdateTest : MetroWindow
    {
        private Test _test;
        public UpdateTest(Test test)
        {
            InitializeComponent();
            _test = test;
            DataContext = test;

            //Get the criteria
            if (_test.Criteria == null || _test.Criteria.Count < 10)
                _test.Criteria = Configuration.Criteria.Select(x => new Criterion(x)).ToList();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _test.ActualTestTime=DateTime.Now;
                BL.FactoryBl.GetObject.UpdateTest(_test);
                Close();
            }
            catch (Exception ex)
            {
                ExceptionMessage.Show(ex.Message, ex.ToString());
            }
        }
    }
}
