using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using App.CustomConsole;

namespace App.Sockets
{
    public class SocketController
    {
        private IPEndPoint ipEndPoint;
        private Socket socket;
        private ManualResetEvent resetEvent;
        public SocketController(int port, string ipAddress = "default")
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = ipHost.AddressList[0];

            if (ipAddress != "default")
                ip = IPAddress.Parse(ipAddress);

            ipEndPoint = new IPEndPoint(ip, port);
            socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            resetEvent = new ManualResetEvent(false);

            socket.Bind(ipEndPoint);
        }

        public void Start()
        {
            MyConsole.Info($"Started listening on { ipEndPoint.Address }:{ ipEndPoint.Port }..");
            socket.Listen(ipEndPoint.Port);

            while(true)
            {
                resetEvent.Reset();

                socket.BeginAccept(new AsyncCallback(Accept), socket);

                resetEvent.WaitOne();
            }
        }

        private void Accept(IAsyncResult result)
        {
            Socket connection = (Socket)result.AsyncState;
            Socket handler = connection.EndAccept(result);

            MyConsole.Info($"{ handler.LocalEndPoint.ToString() } has connected.");

            resetEvent.Set();
        }
    }
}
