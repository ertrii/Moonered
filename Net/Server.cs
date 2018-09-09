using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Net
{
    public class Server
    {
        public bool ServerOn { get; set; } = false;
        private Socket client { get; set; }
        private Socket server { get; set; }
        private string IP { get; set; }
        private int listen { get; set; }

        public Server(string IP, int listen)
        {
            this.IP = IP;//example 192.168.100.4
            this.listen = listen;
        }

        public void Create()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP Server
            IPEndPoint host = new IPEndPoint(IPAddress.Parse(IP), 8082);
            server.Bind(host);//bind
            server.Listen(listen);//quantity client
            ServerOn = true; // Server On...

            client = server.Accept();//waiting...

            //System.Threading.Timer timer = new Timer(listen, 10, 1, 1000);

        }

        public void Listen(object args)
        {
            byte[] byteInMsg = new byte[255];
            try
            {
                int length = client.Receive(byteInMsg, 0, byteInMsg.Length, 0);

                Array.Resize(ref byteInMsg, length);

                Console.WriteLine("<-- " + Encoding.Default.GetString(byteInMsg));
            }
            catch (SocketException)
            {
                Console.Write("f3");
            }
        }

        public void Close()
        {
            server.Close();
        }
    }
}
