using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day2 : Day
    {
        private const string FirstGroupName = "f";
        private const string SecondGroupName = "s";
        private const string CharGroupName = "c";
        private const string PasswordGroupName = "p";

        public override object SolvePart1() => GetInputRecords().Count(record =>
        {
            var count = record.Password.Count(c => c == record.Char);
            return count >= record.First && count <= record.Second;
        });

        public override object SolvePart2() => GetInputRecords().Count(record =>
        {
            var firstChar = record.Password[record.First - 1];
            var secondChar = record.Password[record.Second - 1];
            return (firstChar == record.Char && secondChar != record.Char) ||
                    (firstChar != record.Char && secondChar == record.Char);
        });

        private IEnumerable<(int First, int Second, char Char, string Password)> GetInputRecords()
        {
            var regex = new Regex($@"(?<{FirstGroupName}>\d+)-(?<{SecondGroupName}>\d+) (?<{CharGroupName}>\w): (?<{PasswordGroupName}>\w+)");
            return InputData.GetInputLines(true).Select(line =>
            {
                var match = regex.Match(line);
                return (match.GetInt32Group(FirstGroupName),
                        match.GetInt32Group(SecondGroupName),
                        match.GetCharGroup(CharGroupName),
                        match.GetStringGroup(PasswordGroupName));
            });
        }
    }
}
