using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PLWPF.UserControls
{
    /// <summary>
    /// control to select hours in a day
    /// </summary>
    public partial class TimePicker : UserControl
    {
        /// <summary>
        /// Get or set the selected hour
        /// </summary>
        public int SelectedHour
        {
            set { ComboxTime.SelectedItem = string.Format("{0:00}:00", value);}
            get { return int.Parse(ComboxTime.SelectedItem.ToString().Substring(0, 2)); }
        }

        private bool[] hoursToShow=new bool[24];

        /// <summary>
        /// Get or set the hours to show
        /// </summary>
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

        /// <summary>
        /// Called on selection changed
        /// </summary>
        public event EventHandler SelectionChanged;

        //ctor
        public TimePicker()
        {
            InitializeComponent();

            //initilaze a new hours list
            var listHours = new List<string>();
            int i = 0;
            for (;i<24;i++)
            {
                listHours.Add(string.Format("{0:00}:00", i));
                hoursToShow[i] = true;
            }
            ComboxTime.ItemsSource = listHours;
        }

        //forward the selection changed
        private void ComboxTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectionChanged(this, e);
            }
            catch { }
        }

        /// <summary>
        /// Remove all selections
        /// </summary>
        public void ResetSelection()
        {
            ComboxTime.SelectedIndex = -1;
        }
    }
}
