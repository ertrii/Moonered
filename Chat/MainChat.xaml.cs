using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Net;

namespace Chat
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainChat : Window
    {
        private string user { get; set; }
        private dynamic socket { get; set;  }
        public MainChat(string user, string IP, string mode)
        {
            this.user = user;
            if(mode == "server")
            {
                socket = new Server(IP, 5);
            }else
            {
                socket = new Client();
            }
            InitializeComponent();
        }
        /*
        delegate void Hello();
        public void testing(string a)
        {
            Hello hola = new Hello( () => {
                epa("f3");
            });
            hola();
        }*/
        private Thickness spaceEl(int l = 0, int t = 0, int r = 0, int b = 0)
        {
            Thickness thickness = new Thickness();
            thickness.Left = l;
            thickness.Top = t;
            thickness.Right = r;
            thickness.Bottom = b;
            return thickness;
        }

        private void sendMsg(TextBlock textBlock)
        {
            textBlock.Text = txtSend.Text;
            messagePanel.Children.Add(textBlock);
            txtSend.Text = "";
        }
        private TextBlock createTextBlock(HorizontalAlignment alignment)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = alignment;
            textBlock.Background = (Brush)new BrushConverter().ConvertFrom("#FF3C8065");
            textBlock.Foreground = Brushes.White;
            textBlock.Margin = spaceEl(5, 5);
            textBlock.Padding = spaceEl(10, 5, 10, 5);
            textBlock.MaxWidth = messagePanel.Width - 35;
            textBlock.TextWrapping = TextWrapping.Wrap;
            return textBlock;
        }

        private void btn_send_MouseUp(object sender, MouseButtonEventArgs e)
        {
            sendMsg(createTextBlock(HorizontalAlignment.Left));
        }

        private void txtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                sendMsg(createTextBlock(HorizontalAlignment.Left));
            }
        }
    }
}
