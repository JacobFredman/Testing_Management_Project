﻿using System;
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
    /// Interaction logic for ShowList.xaml
    /// </summary>
    public partial class ShowList : Window
    {

        public ShowList()
        {
            InitializeComponent();
        }
        public void ShowListInGrade(IEnumerable<BE.MainObjects.Trainee> list)
        {
            ListDataGrid.ItemsSource = list;
        }
        public void ShowListInGrade(IEnumerable<BE.MainObjects.Tester> list)
        {
            ListDataGrid.ItemsSource = list;
        }
    }
}
    