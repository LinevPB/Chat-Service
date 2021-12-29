using App.Sockets;
using App.CustomConsole;

namespace Server
{
    public static class Server
    {
        public static void Main()
        {
            Console.Clear();
            MyConsole.WriteLine(ConsoleColor.Blue, "Chat server application");
            MyConsole.NewLine();

            SocketController controller = new SocketController(28970);
            controller.Start();
        }
    }
}