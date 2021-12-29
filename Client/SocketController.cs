using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using App.CustomConsole;
using App.Packets;
using App.Clients;

namespace App.Sockets
{
    public class SocketController
    {
        private Socket socket;
        private IPEndPoint ipEndPoint;
        private string clientName;
        private ManualResetEvent resetEvent;
        private ClientStateObject state;
        public bool Connected {
            get
            {
                return socket.Connected;
            }
        }

        public SocketController(string ip, int port, string clientName = "Anonymous")
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.clientName = clientName;
            resetEvent = new ManualResetEvent(false);
            state = new ClientStateObject();
        }

        public bool Connect()
        {
            if (socket.Connected)
                return false;

            MyConsole.Info($"Trying to connect to { ipEndPoint.Address }:{ ipEndPoint.Port }..");

            int tryCount = 0;
            while (!socket.Connected)
            {
                try
                {
                    tryCount++;
                    socket.Connect(ipEndPoint);
                } catch (Exception ex)
                {
                    if (tryCount > 5)
                    {
                        MyConsole.Error("Could not establish a connection with the server.");
                        socket.Close();
                        return false;
                    }

                    MyConsole.Error(ex.Message);
                }
            }

            Packet packet = new Packet();
            packet.WriteInt((int)Packet.PacketType.SEND_RECEIVE_NAME);
            packet.WriteString(clientName);
            packet.Parse();
            Packet.SendData(socket, packet);

            MyConsole.Info("Connected!");
            MyConsole.WriteLine(ConsoleColor.Green, "---------------------");
            MyConsole.NewLine();

            Task.Run(() =>
            {
                StartListening();
            });

            Task.Run(() =>
            {
                StartReadingLines();
            });

            return true;
        }

        public void Disconnect()
        {
            socket.Close();
        }

        public void StartReadingLines()
        {
            while(true)
            {
                if (resetEvent.GetSafeWaitHandle() == null)
                    continue;

                string line = MyConsole.ReadLine(">>");
                Packet packet = new Packet();
                packet.WriteInt((int)Packet.PacketType.SEND_RECEIVE_MESSAGE);
                packet.WriteString(line);
                packet.Parse();
                Packet.SendData(socket, packet);
            }
        }

        public void StartListening()
        {
            while(socket.Connected)
            {
                resetEvent.Reset();

                socket.BeginReceive(state.Buffer, 0, state.BufferSize, 0, new AsyncCallback(Listen), state);

                resetEvent.WaitOne();
            }
        }

        public async void Listen(IAsyncResult result)
        {
            ClientStateObject state = (ClientStateObject)result.AsyncState;

            if (!socket.Connected)
                return;

            int bytes = socket.EndReceive(result);
            if (bytes == 0)
                return;

            List<Tuple<Packet.DataType, string>> tuples;
            tuples = Packet.Decode(bytes, ref state.StringBuilder, ref state.Buffer);

            switch(Packet.ParseInt(tuples[0].Item2))
            {
                case (int)Packet.PacketType.SEND_RECEIVE_MESSAGE:
                    MyConsole.PlayerMessage(tuples[1].Item2, tuples[2].Item2);
                    break;
            }

            resetEvent.Set();
        }
    }
}
