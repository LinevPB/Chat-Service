using System;
using AppConsole;
using Packets;

namespace Client
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            SocketController controller = new SocketController();
            MyConsole.WriteLine("Hello world!");
        }
    }
}