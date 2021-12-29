using System.Net;
using System.Net.Sockets;
using App.CustomConsole;
using App.Clients;
using App.Packets;
using App.ServerScripts;

namespace App.Sockets
{
    public class SocketController
    {
        private const int MAX_USERS = 120;

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
            MyConsole.NewLine();
            socket.Listen(ipEndPoint.Port);

            while(true)
            {
                resetEvent.Reset();

                socket.BeginAccept(new AsyncCallback(Accept), socket);

                resetEvent.WaitOne();
            }
        }

        private int DetermineId()
        {
            List<int> ids = new List<int>();

            foreach(ClientStructure client in clients)
            {
                ids.Add(client.Id);
            }

            for(int i = 0; i < MAX_USERS; i++)
            {
                if (!ids.Contains(i))
                {
                    return i;
                }
            }

            return 0;
        }

        private async void Accept(IAsyncResult result)
        {
            Socket connection = (Socket)result.AsyncState;
            Socket handler = connection.EndAccept(result);

            ClientStructure client = new ClientStructure(DetermineId(), "Unknown", handler);
            clients.Add(client);

            Scripts.OnConnect(client.Id);
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

            if (!client.Socket.Connected)
                return;

            int bytes = client.Socket.EndReceive(result);
            if (bytes == 0)
                return;

            List<Tuple<Packet.DataType, string>> tuples;
            tuples = Packet.Decode(bytes, ref client.state.StringBuilder, ref client.state.Buffer);

            switch(Packet.ParseInt(tuples[0].Item2))
            {
                case (int)Packet.PacketType.SEND_RECEIVE_NAME:
                    client.Name = tuples[1].Item2;
                    Scripts.OnChangeName(client.Id, tuples[1].Item2, client.Name);
                    break;

                case (int)Packet.PacketType.SEND_RECEIVE_MESSAGE:
                    if (tuples[1].Item2[0] == '/')
                    {
                        string commandName = "";
                        for(int i = 1; i < tuples[1].Item2.Length; i++)
                        {
                            if (tuples[1].Item2[i] == ' ')
                                break;

                            commandName += tuples[1].Item2[i];
                        }

                        string arguments = tuples[1].Item2.Substring(1 + commandName.Length,
                            tuples[1].Item2.Length - 1 - commandName.Length);
                        if (arguments[0] == ' ')
                            arguments = arguments.Substring(1);

                        Scripts.OnCommand(client.Id, commandName, arguments);
                        break;
                    }

                    Packet packet = new Packet();
                    packet.WriteInt((int)Packet.PacketType.SEND_RECEIVE_MESSAGE);
                    packet.WriteString(client.Name);
                    packet.WriteString(tuples[1].Item2);
                    packet.Parse();

                    foreach(ClientStructure send_client in clients)
                    {
                        Packet.SendData(send_client.Socket, packet);
                    }

                    Scripts.OnSendMessage(client.Id, tuples[1].Item2);
                    break;
            }

            client.clientEvent.Set();
        }
    }
}
