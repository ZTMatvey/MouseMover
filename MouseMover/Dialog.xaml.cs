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

namespace MouseMover
{
    public partial class Dialog : Window
    {
        public Dialog(string msg)
        {
            InitializeComponent();
            MessageTB.Text = msg;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => Close();
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
