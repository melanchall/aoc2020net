using System;
using System.Linq;
using Aoc2020Net.Utilities;

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
            .Select(int.Parse)
            .ToArray();

        public int[] GetCommaSeparatedInt32Numbers() => _input
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();

        public long[] GetInputInt64NumbersFromLines() => GetInputLines()
            .Select(long.Parse)
            .ToArray();

        public (T[,] Grid, int Width, int Height) GetInputGrid<T>() where T : Enum => GetGrid<T>(GetInputLines());

        public string[] GetInputGroupsAsJoinedStrings() =>
            string.Join(" ", GetInputLines().Select(l => string.IsNullOrWhiteSpace(l) ? "$$" : l))
                .Split("$$", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        public string[][] GetInputLinesGroups() =>
            string.Join("%%", GetInputLines().Select(l => string.IsNullOrWhiteSpace(l) ? "$$" : l))
                .Split("$$", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(g => g.Split("%%", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

        public static (T[,] Grid, int Width, int Height) GetGrid<T>(string[] lines)
            where T : Enum
        {
            var rules = ReflectionUtilities
                .GetAttributedEnumValues<T, GridSymbolAttribute>()
                .ToDictionary(v => v.Attribute.Symbol, v => v.Value);

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
