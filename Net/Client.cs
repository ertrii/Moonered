﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Net
{
    public class Client
    {
        private Socket client { get; set; }
        private IPEndPoint IPHost { get; set; }

        public void CreateHost(string IP)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IP Server
            IPHost = new IPEndPoint(IPAddress.Parse(IP), 8082);
            //ConnectHost();
        }
        public bool ConnectHost()
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

        public bool SendMsg(string msg)
        {
            byte[] msgInByte = Encoding.Default.GetBytes(msg);
            try
            {
                client.Send(msgInByte, 0, msgInByte.Length, 0);
            }
            catch (SocketException)
            {
                client.Close();
                return false;
            }
            return true;
        }
    }
}

