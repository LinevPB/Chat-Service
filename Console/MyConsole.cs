namespace App.CustomConsole
{
    public static class MyConsole
    {
        public static void Write(string value)
        {
            Console.Write(value);
        }

        public static void Write(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Write(value);
            Console.ResetColor();
        }

        public static void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void NewLine()
        {
            Console.WriteLine("");
        }

        private static void Log(ConsoleColor color, string logType, string logContent)
        {
            Write(color, $"[{ logType }] ");
            WriteLine(logContent);
        }

        public static void Debug(string value)
        {
            Log(ConsoleColor.White, "Debug", value);
        }

        public static void Info(string value)
        {
            Log(ConsoleColor.Yellow, "Info", value);
        }

        public static void Warn(string value)
        {
            Log(ConsoleColor.Magenta, "Warn", value);
        }

        public static void Error(string value)
        {
            Log(ConsoleColor.Red, "Error", value);
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}