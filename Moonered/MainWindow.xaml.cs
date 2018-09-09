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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chat;
namespace Moonered
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void lblClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void btn_Connect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainChat chat = new MainChat("Erick", "192.168.100.4");
            this.Hide();
            chat.Show();
        }
    }
}
