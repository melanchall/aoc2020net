using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020Net.Days
{
    internal sealed class Day10 : Day
    {
        public override object SolvePart1()
        {
            var joltages = GetInputSortedJoltages();

            var differencies = new List<int>();
            joltages.Aggregate(0, (result, j) =>
            {
                differencies.Add(j - result);
                return j;
            });

            return differencies.Count(d => d == 1) * (differencies.Count(d => d == 3) + 1);
        }

        public override object SolvePart2()
        {
            var joltages = GetInputSortedJoltages();
            var maxJoltage = joltages.Max() + 3;
            return CountChains(0, joltages, maxJoltage, new Dictionary<int, long>());
        }

        private long CountChains(int joltage, int[] joltages, int maxJoltage, Dictionary<int, long> counts)
        {
            if (counts.TryGetValue(joltage, out var count))
                return count;

            var nextJoltages = Enumerable.Range(1, 3).Select(i => joltage + i).Where(j => Array.BinarySearch(joltages, j) >= 0);
            if (!nextJoltages.Any())
                return Convert.ToInt32(joltage + 3 == maxJoltage);

            return counts[joltage] = nextJoltages.Select(j => CountChains(j, joltages, maxJoltage, counts)).Sum();
        }

        private int[] GetInputSortedJoltages() => InputData.GetInputInt32NumbersFromLines().OrderBy(j => j).ToArray();
    }
}
