using System;

namespace MMBotDownloader.Utils
{
    internal class UserInterface
    {
        private readonly ConsoleColor _promptColor = ConsoleColor.White;
        private readonly ConsoleColor _inputColor = ConsoleColor.Green;

        private readonly object _lock = new();

        public string Prompt(string text)
        {
            lock (_lock)
            {
                Write(text.TrimEnd() + ' ');
                return Color(_inputColor, Console.ReadLine);
            }
        }

        public void WriteSelection(string text, string value)
        {
            lock (_lock)
            {
                Write(text.TrimEnd() + ' ');
                WriteLine(value, _inputColor);
            }
        }

        public void Write(string text = null, ConsoleColor? color = null)
        {
            lock (_lock)
            {
                Color(color ?? _promptColor, () => Console.Write(text));
            }
        }

        public void WriteLine(string text = null, ConsoleColor? color = null)
        {
            lock (_lock)
            {
                Color(color ?? _promptColor, () => Console.WriteLine(text));
            }
        }

        public T Color<T>(ConsoleColor color, Func<T> func)
        {
            lock (_lock)
            {
                T result = default;
                Color(color, () => { result = func(); });
                return result;
            }
        }

        public void Color(ConsoleColor color, Action action)
        {
            lock (_lock)
            {
                var def = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = color;
                    action();
                }
                finally
                {
                    Console.ForegroundColor = def;
                }
            }
        }
    }
}
