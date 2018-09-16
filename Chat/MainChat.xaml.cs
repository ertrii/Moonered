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
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;

namespace Chat
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainChat : Window
    {
        private string user { get; set; }
        public MainChat(string user)
        {
            this.user = user;
            InitializeComponent();
         
        }

        private HorizontalAlignment alignment { get; set; } = HorizontalAlignment.Left;
        private bool isHost { get; set; }
        private Socket host { get; set; }
        private Socket client { get; set; }
        private bool enabledSendMsg { get; set; } = true;

        public async void createHost(string IP)
        {
            alignment = HorizontalAlignment.Left;
            isHost = true;
            host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            host.Bind(new IPEndPoint(IPAddress.Parse(IP), 8082));//assign
            host.Listen(4);
            sendNotice("Server On");


            Socket clientMsg = await Task.Run( () => host.Accept());
            sendNotice("User Connected");

            receiveClient receiveC = new receiveClient( async (msg) =>
            {
                while (true)
                {
                    byte[] byteMsg = new byte[255];
                    int length = await Task.Run(() => msg.Receive(byteMsg, 0, byteMsg.Length, 0));
                    Array.Resize(ref byteMsg, length);
                    sendMsg(Encoding.Default.GetString(byteMsg));
                }
            });
            
            receiveC(clientMsg);
        }
        //private 
        private delegate void receiveClient(Socket msg);
        
        
        public void createClient(string IP)
        {
            alignment = HorizontalAlignment.Right;
            isHost = false;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectToHost();
        }
        private async void connectToHost()
        {
            await Task.Run(() =>
            {
                try
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse("192.168.100.4"), 8082));
                    enabledSendMsg = true;
                }
                catch (SocketException)
                {
                    enabledSendMsg = false;                    
                }
            });

            btn_send.IsEnabled = enabledSendMsg;

            if (enabledSendMsg)
            {
                sendNotice("Server Conected");
            }
            else
            {
                int timeConnect = 10;
                Label lb = new Label();
                lb.Foreground = Brushes.Gray;
                lb.Content = $"Fail, Connecting in {timeConnect} seconds...";
                lb.HorizontalAlignment = HorizontalAlignment.Center;
                messagePanel.Children.Add(lb);
                
                DispatcherTimer tm = new DispatcherTimer();
                tm.Interval = TimeSpan.FromSeconds(1);
                tm.Tick += (object sender, EventArgs e) => {
                    timeConnect--;
                    this.Dispatcher.Invoke(() => lb.Content = $"Fail, Connecting in {timeConnect} seconds...");
                    if (timeConnect == 0) {
                        tm.Stop();
                        connectToHost();
                        return;
                    }                    
                };
                tm.Start();
            }
            
        }

        private void sendNotice(string text)
        {
            Label lb = new Label();
            lb.Foreground = Brushes.Gray;
            lb.Content = text;
            lb.HorizontalAlignment = HorizontalAlignment.Center;
            lb.Padding = spaceEl(0, 3, 0, 3);
            messagePanel.Children.Add(lb);
        }

        private void sendMsg(string text)
        {            
            if (!isHost) {                
                byte[] msgByte = Encoding.Default.GetBytes(text);
                client.Send(msgByte, 0, msgByte.Length, 0);
            }
            TextBlock textBlock = createTextBlock();
            textBlock.Text = text;
            messagePanel.Children.Add(textBlock);
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
        
        private TextBlock createTextBlock()
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
            sendMsg(txtSend.Text);
        }

        private void txtSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (!enabledSendMsg) return;
            if(e.Key == Key.Enter)
            {
                sendMsg(txtSend.Text);
            }
        }
    }
}
