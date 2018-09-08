using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat.NET
{
    class Client
    {
        private static Socket client { get; set; }
        private static IPEndPoint IPHost { get; set; }

        private void createHost()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP Server
            IPHost = new IPEndPoint(IPAddress.Parse("192.168.100.4"), 8082);
            connectHost();
        }
        public bool connectHost()
        {
            try
            {
                client.Connect(IPHost);//connect to host server
                //"Done conexion."
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
            
        }
        private bool SendMsg(string msg)
        {
            if (msg == "/exit") return false;
            byte[] msgInByte = Encoding.Default.GetBytes(msg);
            try
            {
                client.Send(msgInByte, 0, msgInByte.Length, 0);
            }
            catch (SocketException)
            {
                client.Close();
                createHost();
                return false;
            }

            return true;
        }
    }


}
