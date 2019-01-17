using System.Linq;
using BE;
using BE.MainObjects;
using MahApps.Metro.Controls;

namespace PLWPF.TraineeArea
{
    /// <summary>
    ///     Interaction logic for ShowTestResoults.xaml
    /// </summary>
    public partial class ShowTestResoults : MetroWindow
    {
        /// <summary>
        ///     the test
        /// </summary>
        private readonly Test _test;

        /// <summary>
        ///     Show test Results
        /// </summary>
        /// <param name="test"></param>
        public ShowTestResoults(Test test)
        {
            InitializeComponent();

            //set data context
            _test = test;
            DataContext = test;

            //Get the criteria
            if (_test.Criteria == null || _test.Criteria.Count < 10)
                _test.Criteria = Configuration.Criteria.Select(x => new Criterion(x)).ToList();
        }
    }
}