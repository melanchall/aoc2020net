using System;
using System.Collections.Generic;

namespace Aoc2020Net
{
    internal static class Output
    {
        private enum MessageType
        {
            Info,
            Important,
            Error
        }

        private static readonly Dictionary<MessageType, ConsoleColor> MessagesColors = new Dictionary<MessageType, ConsoleColor>
        {
            [MessageType.Info] = ConsoleColor.White,
            [MessageType.Important] = ConsoleColor.Magenta,
            [MessageType.Error] = ConsoleColor.Red,
        };

        public static void WriteInfo(string message) => WriteMessage(message, MessageType.Info);

        public static void WriteImportant(string message) => WriteMessage(message, MessageType.Important);

        public static void WriteError(string message) => WriteMessage(message, MessageType.Error);

        public static void WriteNewLine() => Console.WriteLine();

        private static void WriteMessage(string message, MessageType messageType)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = MessagesColors[messageType];
            Console.Write(message);
            Console.ForegroundColor = oldColor;
        }
    }
}
