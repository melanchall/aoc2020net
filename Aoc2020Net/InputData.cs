using System;
using System.Linq;
using System.Reflection;

namespace Aoc2020Net
{
    public sealed class InputData
    {
        private readonly string _input;

        public InputData(string input) => _input = input.Trim();

        public string GetInputText() => _input;

        public string[] GetInputLines(bool skipEmpty = false) => _input
            .Split(
                Environment.NewLine,
                StringSplitOptions.TrimEntries)
            .Select(line => line.Trim())
            .Where(line => !skipEmpty || !string.IsNullOrWhiteSpace(line))
            .ToArray();

        public int[] GetInputInt32NumbersFromLines() => GetInputLines()
            .Select(line => int.Parse(line))
            .ToArray();

        public (T[,] Grid, int Width, int Height) GetInputGrid<T>()
            where T : Enum
        {
            var rules = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(f => f.GetCustomAttribute<GridSymbolAttribute>().Symbol, f => (T)f.GetValue(null));

            var lines = GetInputLines();
            var width = lines.First().Length;
            var height = lines.Length;

            var grid = new T[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var symbol = lines[y][x];
                    if (!rules.TryGetValue(symbol, out var value))
                        throw new InvalidOperationException($"Failed to build a grid due to no rule for '{symbol}' symbol.");

                    grid[x, y] = value;
                }
            }

            return (grid, width, height);
        }
    }
}
