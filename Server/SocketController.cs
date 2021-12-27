using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using App.CustomConsole;
using App.Clients;
using App.Packets;

namespace App.Sockets
{
    public class SocketController
    {
        private IPEndPoint ipEndPoint;
        private Socket socket;
        private ManualResetEvent resetEvent;
        private List<ClientStructure> clients;
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

            clients = new List<ClientStructure>();
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

        private async void Accept(IAsyncResult result)
        {
            Socket connection = (Socket)result.AsyncState;
            Socket handler = connection.EndAccept(result);

            MyConsole.Info($"{ handler.LocalEndPoint.ToString() } has connected.");

            ClientStructure client = new ClientStructure(clients.Count, "Unknown", handler);
            clients.Add(client);

            ListenClient(client);

            resetEvent.Set();
        }

        private async void ListenClient(ClientStructure client)
        {
            await Task.Run(() =>
            {
                while (client.Socket.Connected)
                {
                    client.clientEvent.Reset();

                    client.Socket.BeginReceive(client.state.Buffer, 0, client.state.BufferSize, 0,
                        new AsyncCallback(BeginReceive), client);

                    client.clientEvent.WaitOne();
                }
            });
        }

        private async void BeginReceive(IAsyncResult result)
        {
            ClientStructure client = (ClientStructure)result.AsyncState;

            int bytes = client.Socket.EndReceive(result);
            if (bytes == 0)
                return;

            List<Tuple<Packet.DataType, string>> tuples;
            tuples = Packet.Decode(bytes, ref client.state.StringBuilder, ref client.state.Buffer);

            /*switch(Packet.ParseInt(tuples[0].Item2))
            {
                case (int)Packet.PacketType.SEND_RECEIVE_NAME:
                    Console.WriteLine(tuples[1].Item2);
                    break;
            }*/

            client.clientEvent.Set();
        }
    }
}
