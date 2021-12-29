using App.Sockets;
using App.CustomConsole;

namespace Server
{
    public static class Server
    {
        public static void Main()
        {
            MyConsole.WriteLine(ConsoleColor.Blue, "Chat server application");
            MyConsole.NewLine();

            SocketController controller = new SocketController(28970, "127.0.0.1");
            controller.Start();
        }
    }
}