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
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.IO;

namespace Moonered.net
{
    /// <summary>
    /// Lógica de interacción para chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        private string user { get; set; }
        DispatcherTimer timer { get; set; }
        public Chat(string user)
        {
            this.user = user;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            InitializeComponent();

        }

        private TcpListener server { get; set; }
        private TcpClient client { get; set; }
        private StreamReader reader { get; set; }
        private StreamWriter writer { get; set; }
        private bool enabledSendMsg { get; set; } = false;
        //host
        public async void createHost(string IP)
        {
            server = new TcpListener(new IPEndPoint(IPAddress.Parse(IP), 8082));
            server.Start();
            showNotice("Server On");

            client = await Task.Run(() => server.AcceptTcpClient());
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            string nickClient = await Task.Run(() => reader.ReadLine());
            enabledSendMsg = true;
            showNotice(nickClient + " Conected.");

            while (true)
            {
                string msg = await Task.Run(() =>
                {
                    try
                    {
                        return reader.ReadLine();
                    }
                    catch (IOException)
                    {
                        return "";
                    }
                });

                if (msg == "")
                {
                    showNotice(nickClient + " Disconnected.");
                    break;
                }
                showMsg($"{nickClient}: {msg}");
            }

        }
        private void showNotice(string text)
        {
            Label lb = new Label();
            lb.Foreground = Brushes.Gray;
            lb.Content = text;
            lb.HorizontalAlignment = HorizontalAlignment.Center;
            lb.Padding = spaceEl(0, 3, 0, 3);
            messagePanel.Children.Add(lb);
        }

        private void showMsg(string text)
        {
            TextBlock textBlock = createTextBlock(HorizontalAlignment.Right, "#2B3544");
            textBlock.Text = text;
            messagePanel.Children.Add(textBlock);
        }
        private void sendMsg(string text)
        {
            if (!enabledSendMsg) return;
            TextBlock textBlock = createTextBlock(HorizontalAlignment.Left, "#3C8065");
            writer.WriteLine(text);
            textBlock.Text = text;
            messagePanel.Children.Add(textBlock);
            writer.Flush();
            txtSend.Text = "";
        }

        private Thickness spaceEl(int l = 0, int t = 0, int r = 0, int b = 0)
        {
            Thickness thickness = new Thickness();
            thickness.Left = l;
            thickness.Top = t;
            thickness.Right = r;
            thickness.Bottom = b;
            return thickness;
        }

        private TextBlock createTextBlock(HorizontalAlignment alignment, string color)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.HorizontalAlignment = alignment;
            textBlock.Background = (Brush)new BrushConverter().ConvertFrom(color);
            textBlock.Foreground = Brushes.White;
            textBlock.Margin = spaceEl(5, 5);
            textBlock.Padding = spaceEl(10, 5, 10, 5);
            textBlock.MaxWidth = messagePanel.Width - 35;
            textBlock.TextWrapping = TextWrapping.Wrap;
            return textBlock;
        }

        private void btn_send_MouseUp(object sender, MouseButtonEventArgs e)
        {
            sendMsg(txtSend.Text);
        }

        private void txtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (!enabledSendMsg) return;
            if (e.Key == Key.Enter)
            {
                sendMsg(txtSend.Text);
            }
        }
    }
}
