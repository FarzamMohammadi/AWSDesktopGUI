﻿using eBookReader.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eBookReader.Views
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : UserControl
    {
        public SignUpView()
        {
            InitializeComponent();
        }
        private void BackToLogin_OnClick(object sender, RoutedEventArgs e)
        {
            //Changes View to Login View
            Mediator.Notify("GoToLoginView", "");
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            //Changes View to Login View
            Mediator.Notify("GoToLoginView", "");
        }

    }
}
