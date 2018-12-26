using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PLWPF.UserControls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public int SelectedHour
        {
            set { ComboxTime.SelectedItem = string.Format("{0:00}:00", value);}
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
                        listHours.Add(string.Format("{0:00}:00", i));
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
                listHours.Add(string.Format("{0:00}:00", i));
                hoursToShow[i] = true;
            }

            ComboxTime.ItemsSource = listHours;
            

        }

        private void ComboxTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectionChanged(this, e);
            }
            catch { }
        }
    }
}
