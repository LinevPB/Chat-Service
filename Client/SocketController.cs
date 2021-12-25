using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using App.CustomConsole;
using App.Packets;

namespace App.Sockets
{
    public class SocketController
    {
        private Socket socket;
        private IPEndPoint ipEndPoint;
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

            MyConsole.Info("Connected!");
            MyConsole.WriteLine(ConsoleColor.Green, "---------------------");
            MyConsole.NewLine();

            return true;
        }

        public void Disconnect()
        {
            socket.Close();
        }

        public async void ReadAndListen()
        {

        }

        public async void SendData(Packet packet)
        {
            await Task.Run(() =>
            {
                socket.Send(packet.GetData());
            });
        }
    }
}
