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
using Moonered_client.net;

namespace Moonered_client
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
            Hide();
            Chat_client chat = new Chat_client(txtNameUser.Text);
            chat.createClient("192.168.100.4");
            chat.ShowDialog();

        }
    }
}
