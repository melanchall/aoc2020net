using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Aoc2020Net.Utilities;

namespace Aoc2020Net.Days
{
    internal sealed class Day19 : Day
    {
        public override object SolvePart1() => CountValidMessages();

        public override object SolvePart2() => CountValidMessages((8, "42 | 42 8"), (11, "42 31 | 42 11 31"));

        private int CountValidMessages(params (int RuleNumber, string Rule)[] rulesOverridings)
        {
            var linesGroups = InputData.GetInputLinesGroups();
            var rulesLines = linesGroups[0];
            var messagesLines = linesGroups[1];

            var ruleRegex = new Regex(@"(\d+): (.+)");
            var rules = rulesLines
                .Select(line => ruleRegex.Match(line))
                .ToDictionary(m => m.GetInt32Group(1), m => m.GetStringGroup(2).Trim('"'));

            foreach (var (ruleNumber, rule) in rulesOverridings)
            {
                rules[ruleNumber] = rule;
            }

            var regex = new Regex($"^{GetRegexPattern(0, rules, messagesLines.Max(l => l.Length))}$");
            return messagesLines.Count(m => regex.IsMatch(m));
        }

        private string GetRegexPattern(int ruleNumber, Dictionary<int, string> rules, int maxRecursionDepth) =>
            GetRegexPattern(
                ruleNumber,
                rules,
                new Dictionary<int, string>(),
                maxRecursionDepth,
                rules.ToDictionary(r => r.Key, _ => 0));

        private static string GetRegexPattern(
            int ruleNumber,
            Dictionary<int, string> rules,
            Dictionary<int, string> patterns,
            int maxRecursionDepth,
            Dictionary<int, int> recursionCounters)
        {
            if (patterns.TryGetValue(ruleNumber, out var pattern))
                return pattern;

            if (++recursionCounters[ruleNumber] > maxRecursionDepth)
                return string.Empty;

            var rule = rules[ruleNumber];
            return patterns[ruleNumber] = char.IsLetter(rule[0])
                ? rule
                : "(" + Regex.Replace(rule, @"\d+", m => GetRegexPattern(int.Parse(m.Value), rules, patterns, maxRecursionDepth, recursionCounters))
                             .Replace(" ", string.Empty) + ")";
        }
    }
}
