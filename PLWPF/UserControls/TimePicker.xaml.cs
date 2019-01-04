using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PLWPF.UserControls
{
    /// <summary>
    ///     control to select hours in a day
    /// </summary>
    public partial class TimePicker : UserControl
    {
        private readonly bool[] _hoursToShow = new bool[24];

        //ctor
        public TimePicker()
        {
            InitializeComponent();

            //initialize a new hours list
            var listHours = new List<string>();
            var i = 0;
            for (; i < 24; i++)
            {
                listHours.Add($"{i:00}:00");
                _hoursToShow[i] = true;
            }

            ComboxTime.ItemsSource = listHours;
        }

        /// <summary>
        ///     Get or set the selected hour
        /// </summary>
        public int SelectedHour
        {
            set => ComboxTime.SelectedItem = $"{value:00}:00";
            get => int.Parse(ComboxTime.SelectedItem.ToString().Substring(0, 2));
        }

        /// <summary>
        ///     Get or set the hours to show
        /// </summary>
        public bool[] HourToShow
        {
            set
            {
                var listHours = new List<string>();
                var i = 0;
                for (; i < 24; i++)
                    if (value[i])
                    {
                        listHours.Add($"{i:00}:00");
                        _hoursToShow[i] = true;
                    }

                ComboxTime.ItemsSource = listHours;
            }
            get => _hoursToShow;
        }

        /// <summary>
        ///     Called on selection changed
        /// </summary>
        public event EventHandler SelectionChanged;

        //forward the selection changed
        private void ComBoxTime_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectionChanged(this, e);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Remove all selections
        /// </summary>
        public void ResetSelection()
        {
            ComboxTime.SelectedIndex = -1;
        }
    }
}