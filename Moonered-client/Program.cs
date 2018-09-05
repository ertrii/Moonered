using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace Moonered_client
{
    class Program
    {
        private static Socket client { get; set; }
        private static IPEndPoint IPHost { get; set; }
        static void Main(string[] args)
        {
            createHost();
            Console.ReadKey();
        }
        private static void createHost()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP Server
            IPHost = new IPEndPoint(IPAddress.Parse("192.168.100.4"), 8082);
            connectHost();

            sendMsg();
        }
        private static void connectHost()
        {
            while (true)
            {
                try
                {
                    client.Connect(IPHost);//connect to host server
                    Console.WriteLine("Done conexion.");
                    break;
                }
                catch (SocketException)
                {
                    Console.Write("Fail Conexion. Press to try again...");
                    Console.ReadLine();
                }
            }
        }
        private static void sendMsg()
        {
            while (true)
            {
                Console.Write("--> ");
                string msg = Console.ReadLine();
                if (msg == "/exit") break;
                byte[] msgInByte = Encoding.Default.GetBytes(msg);
                try
                {
                    client.Send(msgInByte, 0, msgInByte.Length, 0);
                }
                catch (SocketException)
                {
                    client.Close();
                    createHost();
                    break;
                }
            }

            client.Close();
        }
    }
}
