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

namespace PLWPF.Admin.TestLists
{
    /// <summary>
    /// show a Ienumerablbe in a DataGrid
    /// </summary>
    public partial class ShowList : Window
    {

        public ShowList(object list)
        {
            InitializeComponent();
            ListDataGrid.ItemsSource = (System.Collections.IEnumerable)list;
        }
    
    }
}
    