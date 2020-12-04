using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Aoc2020Net
{
    public abstract class Day
    {
        private InputData _inputData;

        public string Input { get; set; }

        public object[] Parameters { get; set; }

        public int DayNumber
        {
            get
            {
                var typeName = GetType().Name;

                var match = Regex.Match(typeName, @"Day(\d+)");
                if (!match.Success)
                    throw new InvalidOperationException("Failed to extract day number from class name.");

                return int.Parse(match.Groups[1].Value);
            }
        }

        public InputData InputData
        {
            get
            {
                if (_inputData != null)
                    return _inputData;

                var input = Input;
                if (string.IsNullOrWhiteSpace(input))
                {
                    var fileName = $"Inputs\\{GetType().Name}.txt";
                    if (!File.Exists(fileName))
                        throw new InvalidOperationException($"File '{fileName}' is missed.");

                    input = File.ReadAllText(fileName);
                    if (string.IsNullOrWhiteSpace(input))
                        throw new InvalidOperationException($"Content of the file '{fileName}' is empty.");
                }

                return _inputData = new InputData(input);
            }
        }

        public abstract object SolvePart1();

        public abstract object SolvePart2();
    }
}
