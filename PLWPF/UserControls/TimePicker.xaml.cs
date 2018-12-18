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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PLWPF.UserControls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public int SelectedHour
        {
            set
            {
                var time = $"{value:00}:00";
                ComboxTime.SelectedItem = time;
            }
            get { return int.Parse(ComboxTime.SelectedItem.ToString().Substring(0, 2)); }
        }
        private bool[] hoursToShow=new bool[24];
        public bool[] HourToShow
        {
            set
            {
                var listHours = new List<string>();
                int i = 0;
                for (; i < 24; i++)
                {
                    if (value[i])
                    {
                        listHours[i] = $"{i:00}:00";
                        hoursToShow[i] = true;
                    }
                }
                ComboxTime.ItemsSource = listHours;
            }
            get => hoursToShow;
        }

        public event EventHandler SelectionChanged;
        public TimePicker()
        {
            InitializeComponent();
            var listHours = new List<string>();
            int i = 0;
            for (;i<24;i++)
            {
                listHours.Add($"{i:00}:00");
                hoursToShow[i] = true;
            }

            ComboxTime.ItemsSource = listHours;
            

        }

        private void ComboxTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged(this, e);
        }
    }
}
