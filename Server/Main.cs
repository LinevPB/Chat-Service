using App.Sockets;
using App.CustomConsole;

namespace Server
{
    public static class Server
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            MyConsole.WriteLine(ConsoleColor.Blue, "Chat server application");
            MyConsole.NewLine();

            string ip = "default";
            if (args.Length > 0)
                ip = args[0];

            SocketController controller = new SocketController(28970, ip);
            controller.Start();
        }
    }
}