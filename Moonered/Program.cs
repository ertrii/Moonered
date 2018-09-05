using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Moonered
{
    class Program
    {
        private static Socket client { get; set; }
        private static Socket server { get; set; }
        static void Main(string[] args)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP Server
            IPEndPoint host = new IPEndPoint(IPAddress.Parse("192.168.100.4"), 8082);
            server.Bind(host);//bind
            server.Listen(4);//quantity client
            Console.WriteLine("Server On...");

            client = server.Accept();
            Console.WriteLine("Sucsess Conexion with client.");
            
            System.Threading.Timer timer = new Timer(listen, 10,1,1000);

            //closing socket            
            if(Console.ReadLine() == "/exit")
            {
                //timer.sto
                server.Close();
            }
            Console.ReadKey();
        }

        static void listen(object args)
        {
            //sendMsgToClient();
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
        static void sendMsgToClient()
        {
            Console.Write("--> ");
            string hostMsg = Console.ReadLine();
            Console.WriteLine(hostMsg);
        }
    }
}
