namespace App.CustomConsole
{
    public static class MyConsole
    {
        private static int currentLine = 0;
        private static bool readingLine = false;

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
            Console.CursorTop = currentLine;
            currentLine++;
            Console.WriteLine(value);
        }

        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            WriteLine(value);
            Console.ResetColor();
        }

        public static void NewLine()
        {
            WriteLine("");
        }

        public static void PlayerMessage(string name, string msg)
        {
            int cursorLeft = MoveBufferIfReading();
            Console.CursorTop = currentLine;
            Console.CursorLeft = 0;
            Write(ConsoleColor.Yellow, name + "> ");
            WriteLine(msg);
            Console.CursorLeft = cursorLeft;
        }

        private static void Log(ConsoleColor color, string logType, string logContent)
        {
            int cursorLeft = MoveBufferIfReading();
            Write(color, $"[{ logType }] ");
            WriteLine(logContent);
            Console.CursorLeft = cursorLeft;
        }

        public static void Debug(string value)
        {
            Log(ConsoleColor.White, "Debug", value);
        }

        public static void Info(string value)
        {
            Log(ConsoleColor.White, "Info", value);
        }

        public static void Warn(string value)
        {
            Log(ConsoleColor.Magenta, "Warn", value);
        }

        public static void Error(string value)
        {
            Log(ConsoleColor.Red, "Error", value);
        }

        private static void ClearPreviousLine()
        {
            int currentCursorLeft = Console.CursorLeft;
            Console.CursorLeft = 0;
            Console.CursorTop = currentLine;
            Console.Write(new String(' ', Console.WindowWidth));
        }

        public static string ReadLine(bool inline = false)
        {
            readingLine = true;
            string line = Console.ReadLine();
            if (!inline)
                ClearPreviousLine();
            readingLine = false;
            return line;
        }

        public static string ReadLine(string prefix)
        {
            readingLine = true;
            Write(ConsoleColor.Red, prefix + " ");
            string line = ReadLine();
            return line;
        }

        private static int MoveBufferIfReading()
        {
            if (readingLine)
            {
                int cursorLeft = Console.CursorLeft;
                Console.MoveBufferArea(0, Console.CursorTop,
                    Console.BufferWidth, 1, 0, currentLine + 1);
                Console.CursorLeft = cursorLeft;
                return cursorLeft;
            }

            return 0;
        }
    }
}