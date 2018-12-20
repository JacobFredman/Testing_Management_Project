using System.Collections;
using System.Windows;

namespace PLWPF.Admin.TestLists
{
    /// <summary>
    ///     show a Ienumerablbe in a DataGrid
    /// </summary>
    public partial class ShowList : Window
    {
        public ShowList(object list)
        {
            InitializeComponent();
            ListDataGrid.ItemsSource = (IEnumerable) list;
        }
    }
}