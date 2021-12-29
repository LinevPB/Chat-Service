using App.CustomConsole;
using App.Sockets;

namespace Client
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            MyConsole.WriteLine(ConsoleColor.Blue, "Chat application");
            MyConsole.NewLine();

            MyConsole.Write("Enter your name: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string name = MyConsole.ReadLine(true);
            Console.ResetColor();
            MyConsole.NewLine();

            SocketController controller = new SocketController("127.0.0.1", 28970, name);

            if (!controller.Connect())
                return;

            while(controller.Connected)
            {
                controller.StartReadingLines();
            }
        }
    }
}