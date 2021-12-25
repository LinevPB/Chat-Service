using System;
using App.CustomConsole;
using App.Packets;
using App.Sockets;

namespace Client
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            MyConsole.WriteLine(ConsoleColor.Blue, "Chat application");
            MyConsole.NewLine();

            SocketController controller = new SocketController("127.0.0.1", 28970);

            if (!controller.Connect())
            {
                MyConsole.NewLine();
                MyConsole.WriteLine("Press any key to close this window..");
                Console.ReadKey();

                return;
            }

            while (controller.Connected)
            {
               controller.ReadAndListen();
            }
        }
    }
}