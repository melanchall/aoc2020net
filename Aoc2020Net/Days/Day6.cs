using System;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day6 : Day
    {
        public override object SolvePart1() => SumQuestions((_, _) => true);

        public override object SolvePart2() => SumQuestions((g, s) => g.All(l => l.Contains(s)));

        private int SumQuestions(Func<string[], char, bool> predicate) => InputData.GetInputGroupsAsJoinedStrings()
            .Select(l => l.Split(" "))
            .Sum(g => g.SelectMany(l => l).Distinct().Count(s => predicate(g, s)));
    }
}
